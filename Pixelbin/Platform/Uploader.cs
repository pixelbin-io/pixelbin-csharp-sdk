using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pixelbin.Common;
using Pixelbin.Common.Exceptions;
using AccessEnum = Pixelbin.Platform.Enums.AccessEnum;

namespace Pixelbin.Platform
{
    public class Uploader
    {
        private readonly Assets _assets;

        public Uploader(Assets assets)
        {
            _assets = assets;
        }

        public async Task<object> Upload(
            object file,
            string name,
            string path,
            string format,
            AccessEnum access,
            List<string> tags,
            Dictionary<string, object> metadata,
            bool overwrite,
            bool filenameOverride,
            int expiry,
            UploadOptions? uploadOptions = null)
        {
            uploadOptions ??= new UploadOptions();

            // Validate and set default options

            ValidateUploadOptions(uploadOptions);

            var presignedUrl = _assets.createSignedUrlV2(
                name,
                path,
                format,
                access,
                tags,
                metadata,
                overwrite,
                filenameOverride,
                expiry
            ) ?? throw new PixelbinServerResponseError("Error generating signed url");

            return await MultipartUploadToPixelbin(presignedUrl.presignedUrl.url ?? "", presignedUrl.presignedUrl.fields ?? new Dictionary<string, object>(), file, uploadOptions);
        }

        private async Task<object> MultipartUploadToPixelbin(string url, Dictionary<string, object> fields, object file, UploadOptions options)
        {
            var rc = new RequestContext
            {
                Url = url,
                Fields = fields,
                ChunkSize = options.ChunkSize.Value,
                MaxRetries = options.MaxRetries.Value,
                Concurrency = options.Concurrency.Value,
                ExponentialFactor = options.ExponentialFactor.Value,
                PartNumber = 0
            };

            if (file is Stream stream)
            {
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var buffer = memoryStream.ToArray();
                await ProcessBuffer(buffer, rc);
            }
            else if (file is byte[] buffer)
            {
                await ProcessBuffer(buffer, rc);
            }
            else if (file is string filePath && File.Exists(filePath))
            {
                var bytesBuffer = await File.ReadAllBytesAsync(filePath);
                await ProcessBuffer(bytesBuffer, rc);
            }
            else
            {
                throw new ArgumentException("Unsupported file type. Expected Stream, byte array, or valid file path.");
            }

            return await CompleteMultipartUpload(rc);
        }

        private async Task UploadChunk(string url, Dictionary<string, object> fields, byte[] chunk, int partNumber, int maxRetries, double exponentialFactor)
        {
            var retryCount = 0;
            var delay = 1000; // Start with 1 second delay

            while (true)
            {
                try
                {
                    using var formData = new MultipartFormDataContent();
                    foreach (var field in fields)
                    {
                        formData.Add(new StringContent(field.Value.ToString()), field.Key);
                    }

                    formData.Add(new ByteArrayContent(chunk), "file", "file");
                    var uploadUrl = new UriBuilder(url);
                    if (uploadUrl.Query.Length > 0)
                        uploadUrl.Query += $"&partNumber={partNumber}";
                    else
                        uploadUrl.Query += $"partNumber={partNumber}";

                    var response = await HttpHelper.Request("PUT", uploadUrl.Uri.ToString(), new Dictionary<string, string>() { { "Content-Type", "multipart/form-data" } }, formData, 15, new Dictionary<string, object>());
                    var statusCode = response["status_code"] as int? ?? 500;
                    if (statusCode != 204)
                    {
                        throw new PixelbinServerResponseError(response["error_message"].ToString(), statusCode);
                    }
                    return;
                }
                catch (HttpRequestException ex)
                {
                    if (retryCount >= maxRetries)
                        throw;

                    if (ex.InnerException is System.Net.WebException webEx &&
                        webEx.Response is System.Net.HttpWebResponse response &&
                        (int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
                        throw;

                    await Task.Delay(delay);
                    delay = (int)(delay * exponentialFactor);
                    retryCount++;
                }
            }
        }

        private async Task<Dictionary<string, object>> CompleteMultipartUpload(RequestContext rc)
        {
            var retryCount = 0;
            var delay = 1000;

            while (true)
            {
                try
                {
                    var parts = Enumerable.Range(1, rc.PartNumber).ToList();
                    var content = new Dictionary<string, object>(rc.Fields)
                    {
                        { "parts", parts }
                    };

                    return await HttpHelper.Request("POST", rc.Url, new Dictionary<string, string>() { { "Content-Type", "application/json" } }, content, 15, new Dictionary<string, object>());
                }
                catch (HttpRequestException ex)
                {
                    if (retryCount >= rc.MaxRetries)
                        throw;

                    if (ex.InnerException is HttpRequestException httpEx && httpEx.Data.Contains("StatusCode"))
                    {
                        var statusCode = (int)httpEx.Data["StatusCode"];
                        if (statusCode >= 400 && statusCode < 500)
                            throw;
                    }

                    await Task.Delay(delay);
                    delay = (int)(delay * rc.ExponentialFactor);
                    retryCount++;
                }
            }
        }

        private void ValidateUploadOptions(UploadOptions options)
        {
            options.ChunkSize ??= 10 * 1024 * 1024;
            if (options.ChunkSize <= 0)
                throw new ArgumentException("Invalid chunkSize: Must be a positive integer");

            options.MaxRetries ??= 2;
            if (options.MaxRetries < 0)
                throw new ArgumentException("Invalid maxRetries: Must be a non-negative integer");

            options.Concurrency ??= 3;
            if (options.Concurrency <= 0)
                throw new ArgumentException("Invalid concurrency: Must be a positive integer");

            options.ExponentialFactor ??= 2;
            if (options.ExponentialFactor < 0)
                throw new ArgumentException("Invalid exponentialFactor: Must be a non-negative number");
        }



        private async Task<byte[]> UploadBufferChunks(byte[] buffer, RequestContext rc)
        {
            while (buffer.Length > 0)
            {
                var tasks = new List<Task>();
                for (var i = 0; i < rc.Concurrency && buffer.Length > 0; i++)
                {
                    var chunkSize = Math.Min(rc.ChunkSize, buffer.Length);
                    var chunk = new byte[chunkSize];
                    Buffer.BlockCopy(buffer, 0, chunk, 0, chunkSize);
                    buffer = buffer.Skip(chunkSize).ToArray();
                    rc.PartNumber++;

                    var task = UploadChunk(rc.Url, rc.Fields, chunk, rc.PartNumber, rc.MaxRetries, rc.ExponentialFactor);
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);
            }

            return buffer;
        }

        private async Task ProcessBuffer(byte[] buffer, RequestContext rc)
        {
            var remainingBuffer = buffer;
            while (remainingBuffer.Length > 0)
            {
                remainingBuffer = await UploadBufferChunks(remainingBuffer, rc);
            }
        }

        private class RequestContext
        {
            public string Url { get; set; }
            public Dictionary<string, object> Fields { get; set; }
            public int ChunkSize { get; set; }
            public int MaxRetries { get; set; }
            public int Concurrency { get; set; }
            public double ExponentialFactor { get; set; }
            public int PartNumber { get; set; }
        }
    }

    public class UploadOptions
    {
        public int? ChunkSize { get; set; }
        public int? MaxRetries { get; set; }
        public int? Concurrency { get; set; }
        public double? ExponentialFactor { get; set; }
    }
}

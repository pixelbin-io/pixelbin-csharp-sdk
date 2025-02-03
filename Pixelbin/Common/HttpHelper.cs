using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Pixelbin.Common.Constants;

namespace Pixelbin.Common
{
    public interface IHttpClientWrapper
    {
        int Timeout { get; set; }
        CookieContainer cookieContainer { get; }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        public int Timeout { get; set; }
        public CookieContainer cookieContainer { get; }
        private HttpClient? client;

        public HttpClientWrapper()
        {
            this.cookieContainer = new CookieContainer();
        }

        public HttpClientWrapper(HttpClient client)
        {
            cookieContainer = new CookieContainer();
            this.client = client;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var handler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer
            };
            if (this.client != null)
            {
                return await this.client.SendAsync(request);
            }
            else
            {
                var client = new HttpClient(handler)
                {
                    Timeout = TimeSpan.FromSeconds(this.Timeout),
                };
                return await client.SendAsync(request);
            }
        }
    }

    public class HttpHelper
    {
        public static IHttpClientWrapper client = new HttpClientWrapper();

        public static async Task<Dictionary<string, object>> Request<T>(string method, string url, Dictionary<string, string> headers, T? data, int timeoutAllowed = HTTP_TIMEOUT, Dictionary<string, object>? queryParams = null) where T : class, new()
        {
            var startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var request = new HttpRequestMessage()
            {
                Method = new HttpMethod(method.ToUpper()),
                RequestUri = new Uri(url)
            };
            client.Timeout = timeoutAllowed;
            foreach (KeyValuePair<string, string> header in headers)
            {
                if (header.Key != "Content-Type")
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            if (queryParams != null && queryParams.Count > 0)
            {
                var queryBuilder = new StringBuilder();
                foreach (KeyValuePair<string, object> param in queryParams)
                {
                    if (param.Value is IEnumerable<string> list)
                    {
                        foreach (string item in list)
                        {
                            queryBuilder.Append(Uri.EscapeDataString(param.Key));
                            queryBuilder.Append('=');
                            queryBuilder.Append(Uri.EscapeDataString(Convert.ToString(item)));
                            queryBuilder.Append('&');
                        }
                    }
                    else
                    {
                        queryBuilder.Append(Uri.EscapeDataString(param.Key));
                        queryBuilder.Append('=');
                        queryBuilder.Append(Uri.EscapeDataString(Convert.ToString(param.Value)));
                        queryBuilder.Append('&');
                    }
                }
                queryBuilder.Length--;
                request.RequestUri = new Uri($"{request.RequestUri}?{queryBuilder}");
            }
            if (data != null)
            {
                if (headers.ContainsKey("Content-Type") && headers["Content-Type"] == "application/json")
                {
                    var contentStr = JsonConvert.SerializeObject(data);
                    var content = new StringContent(contentStr, Encoding.UTF8, "application/json");
                    request.Content = content;
                }
                else
                {
                    var content = new MultipartFormDataContent();

                    if (data is MultipartFormDataContent)
                    {
                        foreach (var dataContent in data as MultipartFormDataContent)
                        {
                            var name = dataContent.Headers.ContentDisposition.Name;
                            if (dataContent is ByteArrayContent)
                            {
                                var value = dataContent.ReadAsByteArrayAsync().Result;
                                if (name == "file")
                                {
                                    if (dataContent is ByteArrayContent)
                                    {
                                        content.Add(new ByteArrayContent(value), "file", name);
                                    }
                                }
                            } else if (dataContent is StreamContent)
                            {
                                var value = dataContent.ReadAsStreamAsync().Result;
                                if (name == "file")
                                {
                                    if (dataContent is ByteArrayContent)
                                    {
                                        content.Add(new StreamContent(value), "file", name);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        FileStream file = (FileStream)data.GetType().GetProperty("file").GetValue(data);
                        string fileName = data.GetType().GetProperty("name") != null ? Convert.ToString(data.GetType().GetProperty("name").GetValue(data)) : "file";
                        string filePath = file.Name;
                        content.Add(new StreamContent(file), "file", file.Name);
                    }

                    var data_without_file = new Dictionary<string, object>();

                    if (data is MultipartFormDataContent)
                    {
                        foreach (var dataContent in data as MultipartFormDataContent)
                        {
                            if (dataContent.Headers.ContentDisposition.Name != "file")
                            {
                                data_without_file.Add(dataContent.Headers.ContentDisposition.Name, dataContent.ReadAsStringAsync().Result);
                            }
                        }
                    } else
                    {
                        data_without_file = data.GetType().GetProperties()
                            .Where(x => x.Name != "file")
                            .ToDictionary(x => x.Name, x => x.GetValue(data));
                    }

                    foreach (var prop in data_without_file)
                    {
                        if (prop.Key == "name")
                            continue;
                        if (prop.Value is string str)
                        {
                            content.Add(new StringContent(str), prop.Key);
                        }
                        else if (prop.Value is bool boolVal)
                        {
                            content.Add(new StringContent(boolVal.ToString().ToLower()), prop.Key);
                        }
                        else if (prop.Value is IEnumerable<string> list)
                        {
                            foreach (var ele in list)
                            {
                                content.Add(new StringContent(ele), prop.Key);
                            }
                        }
                        else if (prop.Value is Dictionary<string, string> dict)
                        {
                            var contentStr = JsonConvert.SerializeObject(dict);
                            content.Add(new StringContent(contentStr, Encoding.UTF8, "application/json"), prop.Key);
                        }
                    }

                    request.Content = content;
                }
            }
            var responseDict = new Dictionary<string, object>() {
                { "url", url },
                { "method", method },
                { "params", queryParams ?? new Dictionary<string, object>() },
                { "data", data ?? new T() },
                { "external_call_request_time", DateTimeHelper.GetIstNow().ToString() },
                { "status_code", 0 },
                { "text", "" },
                { "headers", "" },
                { "cookies", new Dictionary<string, string>() },
                { "error_message", "" },
            };
            try
            {
                var response = await client.SendAsync(request);
                responseDict["status_code"] = (int)response.StatusCode;
                responseDict["headers"] = response.Headers.ToDictionary(h => h.Key, h => h.Value.First());
                responseDict["cookies"] = new Dictionary<string, string>();
                foreach (Cookie cookie in client.cookieContainer.GetCookies(request.RequestUri))
                {
                    Dictionary<string, string> cookies = (Dictionary<string, string>)responseDict["cookies"];
                    cookies.Add(cookie.Name, cookie.Value);
                    responseDict["cookies"] = cookies;
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                responseDict["text"] = responseBody;
                responseDict["content"] = responseBody;
            }
            catch (Exception e)
            {
                responseDict["status_code"] = 999;
                responseDict["text"] = e.Message;
            }
            responseDict["latency"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - startTime;
            return responseDict;
        }
    }
}
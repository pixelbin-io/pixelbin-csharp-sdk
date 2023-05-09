using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pixelbin.Common.Exceptions;
using Pixelbin.Platform.Models;
using AccessEnum = Pixelbin.Platform.Enums.AccessEnum;

namespace Pixelbin.Platform
{
    /// <summary>
    /// PixelbinClient is wrapper class for hitting pixelbin apis
    /// </summary>
    public class PixelbinClient
    {
        public PixelbinConfig config;
        public Assets assets { get; set; }
        public Organization organization { get; set; }
        
        /// <summary>
        /// create instance of PixelbinClient
        /// </summary>
        /// <param name="config">instances of PixelbinConfig</param>
        public PixelbinClient(PixelbinConfig config)
        {
            this.config = config;
            assets = new Assets(config);
            organization = new Organization(config);
        }
    }

    public class Assets
    {
        private readonly PixelbinConfig _configuration;
    
        public Assets(PixelbinConfig configuration)
        {
            _configuration = configuration;
        }
         
        /// <summary>
        /// Upload File
        /// </summary>
        /// <remarks>
        /// Upload File to Pixelbin
        /// </remarks>
        /// <param name="file">Asset file</param>
        /// <param name="path">Path where you want to store the asset. Path of containing folder</param>
        /// <param name="name">Name of the asset, if not provided name of the file will be used. Note - The provided name will be slugified to make it URL safe</param>
        /// <param name="access">Access level of asset, can be either `public-read` or `private`</param>
        /// <param name="tags">Asset tags</param>
        /// <param name="metadata">Asset related metadata</param>
        /// <param name="overwrite">Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.</param>
        /// <param name="filenameOverride">If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised.</param>
        public async Task<UploadResponse?> fileUploadAsync(
            FileStream file, 
            string? path = null, 
            string? name = null, 
            AccessEnum? access = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null, 
            bool? overwrite = null, 
            bool? filenameOverride = null
        )
        { 
    
            // Body
            var body = new FileUploadRequest();
    
            body.file = (FileStream)file;
    
            if (path != null)
            { 
                body.path = (string)path;
            }
    
            if (name != null)
            { 
                body.name = (string)name;
            }
    
            if (access != null)
            { 
                body.access = (AccessEnum)access;
            }
    
            if (tags != null)
            { 
                body.tags = (List<string>)tags;
            }
    
            if (metadata != null)
            { 
                body.metadata = (Dictionary<string, object>)metadata;
            }
    
            if (overwrite != null)
            { 
                body.overwrite = (bool)overwrite;
            }
    
            if (filenameOverride != null)
            { 
                body.filenameOverride = (bool)filenameOverride;
            }
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "post",
                string.Format($"/service/platform/assets/v1.0/upload/direct"),
                null,
                body,
                "multipart/form-data"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<UploadResponse>(response["content"].ToString());
        }
    
        public UploadResponse? fileUpload(
            FileStream file, 
            string? path = null, 
            string? name = null, 
            AccessEnum? access = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null, 
            bool? overwrite = null, 
            bool? filenameOverride = null
        )
        {
            return Task.Run(() => fileUploadAsync(
            file:file, 
            path:path, 
            name:name, 
            access:access, 
            tags:tags, 
            metadata:metadata, 
            overwrite:overwrite, 
            filenameOverride:filenameOverride)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Upload Asset with url
        /// </summary>
        /// <remarks>
        /// Upload Asset with url
        /// </remarks>
        /// <param name="url">Asset URL</param>
        /// <param name="path">Path where you want to store the asset. Path of containing folder.</param>
        /// <param name="name">Name of the asset, if not provided name of the file will be used. Note - The provided name will be slugified to make it URL safe</param>
        /// <param name="access">Access level of asset, can be either `public-read` or `private`</param>
        /// <param name="tags">Asset tags</param>
        /// <param name="metadata">Asset related metadata</param>
        /// <param name="overwrite">Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.</param>
        /// <param name="filenameOverride">If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised.</param>
        public async Task<UploadResponse?> urlUploadAsync(
            string url, 
            string? path = null, 
            string? name = null, 
            AccessEnum? access = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null, 
            bool? overwrite = null, 
            bool? filenameOverride = null
        )
        { 
    
            // Body
            var body = new UrlUploadRequest();
    
            body.url = (string)url;
    
            if (path != null)
            { 
                body.path = (string)path;
            }
    
            if (name != null)
            { 
                body.name = (string)name;
            }
    
            if (access != null)
            { 
                body.access = (AccessEnum)access;
            }
    
            if (tags != null)
            { 
                body.tags = (List<string>)tags;
            }
    
            if (metadata != null)
            { 
                body.metadata = (Dictionary<string, object>)metadata;
            }
    
            if (overwrite != null)
            { 
                body.overwrite = (bool)overwrite;
            }
    
            if (filenameOverride != null)
            { 
                body.filenameOverride = (bool)filenameOverride;
            }
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "post",
                string.Format($"/service/platform/assets/v1.0/upload/url"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<UploadResponse>(response["content"].ToString());
        }
    
        public UploadResponse? urlUpload(
            string url, 
            string? path = null, 
            string? name = null, 
            AccessEnum? access = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null, 
            bool? overwrite = null, 
            bool? filenameOverride = null
        )
        {
            return Task.Run(() => urlUploadAsync(
            url:url, 
            path:path, 
            name:name, 
            access:access, 
            tags:tags, 
            metadata:metadata, 
            overwrite:overwrite, 
            filenameOverride:filenameOverride)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// S3 Signed URL upload
        /// </summary>
        /// <remarks>
        /// For the given asset details, a S3 signed URL will be generated, which can be then used to upload your asset. 
        /// </remarks>
        /// <param name="name">name of the file</param>
        /// <param name="path">Path of containing folder.</param>
        /// <param name="format">Format of the file</param>
        /// <param name="access">Access level of asset, can be either `public-read` or `private`</param>
        /// <param name="tags">Tags associated with the file.</param>
        /// <param name="metadata">Metadata associated with the file.</param>
        /// <param name="overwrite">Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.</param>
        /// <param name="filenameOverride">If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised.</param>
        public async Task<SignedUploadResponse?> createSignedUrlAsync(
            string? name = null, 
            string? path = null, 
            string? format = null, 
            AccessEnum? access = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null, 
            bool? overwrite = null, 
            bool? filenameOverride = null
        )
        { 
    
            // Body
            var body = new SignedUploadRequest();
    
            if (name != null)
            { 
                body.name = (string)name;
            }if (path != null)
            { 
                body.path = (string)path;
            }if (format != null)
            { 
                body.format = (string)format;
            }if (access != null)
            { 
                body.access = (AccessEnum)access;
            }if (tags != null)
            { 
                body.tags = (List<string>)tags;
            }if (metadata != null)
            { 
                body.metadata = (Dictionary<string, object>)metadata;
            }if (overwrite != null)
            { 
                body.overwrite = (bool)overwrite;
            }if (filenameOverride != null)
            { 
                body.filenameOverride = (bool)filenameOverride;
            }
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "post",
                string.Format($"/service/platform/assets/v1.0/upload/signed-url"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<SignedUploadResponse>(response["content"].ToString());
        }
    
        public SignedUploadResponse? createSignedUrl(
            string? name = null, 
            string? path = null, 
            string? format = null, 
            AccessEnum? access = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null, 
            bool? overwrite = null, 
            bool? filenameOverride = null
        )
        {
            return Task.Run(() => createSignedUrlAsync(
            name:name, 
            path:path, 
            format:format, 
            access:access, 
            tags:tags, 
            metadata:metadata, 
            overwrite:overwrite, 
            filenameOverride:filenameOverride)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// List and search files and folders.
        /// </summary>
        /// <remarks>
        /// List all files and folders in root folder. Search for files if name is provided. If path is provided, search in the specified path. 
        /// </remarks>
        /// <param name="name">Find items with matching name</param>
        /// <param name="path">Find items with matching path</param>
        /// <param name="format">Find items with matching format</param>
        /// <param name="tags">Find items containing these tags</param>
        /// <param name="onlyFiles">If true will fetch only files</param>
        /// <param name="onlyFolders">If true will fetch only folders</param>
        /// <param name="pageNo">Page No.</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="sort">Key to sort results by. A "-" suffix will sort results in descending orders. </param>
        public async Task<ListFilesResponse?> listFilesAsync(
            string? name = null,
            string? path = null,
            string? format = null,
            List<string>? tags = null,
            bool? onlyFiles = null,
            bool? onlyFolders = null,
            int? pageNo = null,
            int? pageSize = null,
            string? sort = null
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            if (name != null)
            {
                payload.Add("name", name);
            }
    
            if (path != null)
            {
                payload.Add("path", path);
            }
    
            if (format != null)
            {
                payload.Add("format", format);
            }
    
            if (tags != null)
            {
                payload.Add("tags", tags);
            }
    
            if (onlyFiles != null)
            {
                payload.Add("onlyFiles", onlyFiles);
            }
    
            if (onlyFolders != null)
            {
                payload.Add("onlyFolders", onlyFolders);
            }
    
            if (pageNo != null)
            {
                payload.Add("pageNo", pageNo);
            }
    
            if (pageSize != null)
            {
                payload.Add("pageSize", pageSize);
            }
    
            if (sort != null)
            {
                payload.Add("sort", sort);
            }
    
            // Params
            var query_params = new Dictionary<string, object>();
            
            if (name != null)
            {
                query_params.Add("name", name);
            }
            
            if (path != null)
            {
                query_params.Add("path", path);
            }
            
            if (format != null)
            {
                query_params.Add("format", format);
            }
            
            if (tags != null)
            {
                query_params.Add("tags", tags);
            }
            
            if (onlyFiles != null)
            {
                query_params.Add("onlyFiles", onlyFiles);
            }
            
            if (onlyFolders != null)
            {
                query_params.Add("onlyFolders", onlyFolders);
            }
            
            if (pageNo != null)
            {
                query_params.Add("pageNo", pageNo);
            }
            
            if (pageSize != null)
            {
                query_params.Add("pageSize", pageSize);
            }
            
            if (sort != null)
            {
                query_params.Add("sort", sort);
            }
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/listFiles"),
                query_params,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<ListFilesResponse>(response["content"].ToString());
        }
    
        public ListFilesResponse? listFiles(
            string? name = null,
            string? path = null,
            string? format = null,
            List<string>? tags = null,
            bool? onlyFiles = null,
            bool? onlyFolders = null,
            int? pageNo = null,
            int? pageSize = null,
            string? sort = null
        )
        {
            return Task.Run(() => listFilesAsync(
            name:name, 
            path:path, 
            format:format, 
            tags:tags, 
            onlyFiles:onlyFiles, 
            onlyFolders:onlyFolders, 
            pageNo:pageNo, 
            pageSize:pageSize, 
            sort:sort)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get file details with _id
        /// </summary>
        /// <param name="_id">_id of File</param>
        public async Task<FilesResponse?> getFileByIdAsync(
            string _id
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("_id", _id);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/files/id/{_id}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<FilesResponse>(response["content"].ToString());
        }
    
        public FilesResponse? getFileById(
            string _id
        )
        {
            return Task.Run(() => getFileByIdAsync(
            _id:_id)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get file details with fileId
        /// </summary>
        /// <param name="fileId">Combination of `path` and `name` of file</param>
        public async Task<FilesResponse?> getFileByFileIdAsync(
            string fileId
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("fileId", fileId);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/files/{fileId}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<FilesResponse>(response["content"].ToString());
        }
    
        public FilesResponse? getFileByFileId(
            string fileId
        )
        {
            return Task.Run(() => getFileByFileIdAsync(
            fileId:fileId)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Update file details
        /// </summary>
        /// <param name="fileId">Combination of `path` and `name`</param>
        /// <param name="name">Name of the file</param>
        /// <param name="path">path of containing folder.</param>
        /// <param name="access">Access level of asset, can be either `public-read` or `private`</param>
        /// <param name="isActive">Whether the file is active</param>
        /// <param name="tags">Tags associated with the file</param>
        /// <param name="metadata">Metadata associated with the file</param>
        public async Task<FilesResponse?> updateFileAsync(
            string fileId,
            string? name = null, 
            string? path = null, 
            string? access = null, 
            bool? isActive = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("fileId", fileId);
    
            // Body
            var body = new UpdateFileRequest();
    
            if (name != null)
            { 
                body.name = (string)name;
            }if (path != null)
            { 
                body.path = (string)path;
            }if (access != null)
            { 
                body.access = (string)access;
            }if (isActive != null)
            { 
                body.isActive = (bool)isActive;
            }if (tags != null)
            { 
                body.tags = (List<string>)tags;
            }if (metadata != null)
            { 
                body.metadata = (Dictionary<string, object>)metadata;
            }
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "patch",
                string.Format($"/service/platform/assets/v1.0/files/{fileId}"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<FilesResponse>(response["content"].ToString());
        }
    
        public FilesResponse? updateFile(
            string fileId,
            string? name = null, 
            string? path = null, 
            string? access = null, 
            bool? isActive = null, 
            List<string>? tags = null, 
            Dictionary<string, object>? metadata = null
        )
        {
            return Task.Run(() => updateFileAsync(
            fileId:fileId,
            name:name, 
            path:path, 
            access:access, 
            isActive:isActive, 
            tags:tags, 
            metadata:metadata)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="fileId">Combination of `path` and `name`</param>
        public async Task<FilesResponse?> deleteFileAsync(
            string fileId
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("fileId", fileId);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "delete",
                string.Format($"/service/platform/assets/v1.0/files/{fileId}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<FilesResponse>(response["content"].ToString());
        }
    
        public FilesResponse? deleteFile(
            string fileId
        )
        {
            return Task.Run(() => deleteFileAsync(
            fileId:fileId)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Delete multiple files
        /// </summary>
        /// <param name="ids">Array of file _ids to delete</param>
        public async Task<List<FilesResponse>?> deleteFilesAsync(
            List<string> ids
        )
        { 
    
            // Body
            var body = new DeleteMultipleFilesRequest();
    
            body.ids = (List<string>)ids;
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "post",
                string.Format($"/service/platform/assets/v1.0/files/delete"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<List<FilesResponse>>(response["content"].ToString());
        }
    
        public List<FilesResponse>? deleteFiles(
            List<string> ids
        )
        {
            return Task.Run(() => deleteFilesAsync(
            ids:ids)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Create folder
        /// </summary>
        /// <remarks>
        /// Create a new folder at the specified path. Also creates the ancestors if they do not exist. 
        /// </remarks>
        /// <param name="name">Name of the folder</param>
        /// <param name="path">path of containing folder.</param>
        public async Task<FoldersResponse?> createFolderAsync(
            string name, 
            string? path = null
        )
        { 
    
            // Body
            var body = new CreateFolderRequest();
    
            body.name = (string)name;
    
            if (path != null)
            { 
                body.path = (string)path;
            }
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "post",
                string.Format($"/service/platform/assets/v1.0/folders"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<FoldersResponse>(response["content"].ToString());
        }
    
        public FoldersResponse? createFolder(
            string name, 
            string? path = null
        )
        {
            return Task.Run(() => createFolderAsync(
            name:name, 
            path:path)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get folder details
        /// </summary>
        /// <remarks>
        /// Get folder details 
        /// </remarks>
        /// <param name="path">Folder path</param>
        /// <param name="name">Folder name</param>
        public async Task<ExploreItem?> getFolderDetailsAsync(
            string? path = null,
            string? name = null
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            if (path != null)
            {
                payload.Add("path", path);
            }
    
            if (name != null)
            {
                payload.Add("name", name);
            }
    
            // Params
            var query_params = new Dictionary<string, object>();
            
            if (path != null)
            {
                query_params.Add("path", path);
            }
            
            if (name != null)
            {
                query_params.Add("name", name);
            }
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/folders"),
                query_params,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<ExploreItem>(response["content"].ToString());
        }
    
        public ExploreItem? getFolderDetails(
            string? path = null,
            string? name = null
        )
        {
            return Task.Run(() => getFolderDetailsAsync(
            path:path, 
            name:name)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Update folder details
        /// </summary>
        /// <remarks>
        /// Update folder details. Eg: Soft delete it by making `isActive` as `false`. We currently do not support updating folder name or path. 
        /// </remarks>
        /// <param name="folderId">combination of `path` and `name`</param>
        /// <param name="isActive">whether the folder is active</param>
        public async Task<FoldersResponse?> updateFolderAsync(
            string folderId,
            bool? isActive = null
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("folderId", folderId);
    
            // Body
            var body = new UpdateFolderRequest();
    
            if (isActive != null)
            { 
                body.isActive = (bool)isActive;
            }
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "patch",
                string.Format($"/service/platform/assets/v1.0/folders/{folderId}"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<FoldersResponse>(response["content"].ToString());
        }
    
        public FoldersResponse? updateFolder(
            string folderId,
            bool? isActive = null
        )
        {
            return Task.Run(() => updateFolderAsync(
            folderId:folderId,
            isActive:isActive)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Delete folder
        /// </summary>
        /// <remarks>
        /// Delete folder and all its children permanently. 
        /// </remarks>
        /// <param name="_id">_id of folder to be deleted</param>
        public async Task<FoldersResponse?> deleteFolderAsync(
            string _id
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("_id", _id);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "delete",
                string.Format($"/service/platform/assets/v1.0/folders/{_id}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<FoldersResponse>(response["content"].ToString());
        }
    
        public FoldersResponse? deleteFolder(
            string _id
        )
        {
            return Task.Run(() => deleteFolderAsync(
            _id:_id)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get all ancestors of a folder
        /// </summary>
        /// <remarks>
        /// Get all ancestors of a folder, using the folder ID. 
        /// </remarks>
        /// <param name="_id">_id of the folder</param>
        public async Task<GetAncestorsResponse?> getFolderAncestorsAsync(
            string _id
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("_id", _id);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/folders/{_id}/ancestors"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<GetAncestorsResponse>(response["content"].ToString());
        }
    
        public GetAncestorsResponse? getFolderAncestors(
            string _id
        )
        {
            return Task.Run(() => getFolderAncestorsAsync(
            _id:_id)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Add credentials for a transformation module.
        /// </summary>
        /// <remarks>
        /// Add a transformation modules's credentials for an organization. 
        /// </remarks>
        /// <param name="credentials">Credentials of the plugin</param>
        /// <param name="pluginId">Unique identifier for the plugin this credential belongs to</param>
        public async Task<AddCredentialsResponse?> addCredentialsAsync(
            Dictionary<string, object> credentials, 
            string pluginId
        )
        { 
    
            // Body
            var body = new AddCredentialsRequest();
    
            body.credentials = (Dictionary<string, object>)credentials;body.pluginId = (string)pluginId;
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "post",
                string.Format($"/service/platform/assets/v1.0/credentials"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddCredentialsResponse>(response["content"].ToString());
        }
    
        public AddCredentialsResponse? addCredentials(
            Dictionary<string, object> credentials, 
            string pluginId
        )
        {
            return Task.Run(() => addCredentialsAsync(
            credentials:credentials, 
            pluginId:pluginId)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Update credentials of a transformation module.
        /// </summary>
        /// <remarks>
        /// Update credentials of a transformation module, for an organization. 
        /// </remarks>
        /// <param name="pluginId">ID of the plugin whose credentials are being updated</param>
        /// <param name="credentials">Credentials of the plugin</param>
        public async Task<AddCredentialsResponse?> updateCredentialsAsync(
            string pluginId,
            Dictionary<string, object> credentials
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("pluginId", pluginId);
    
            // Body
            var body = new UpdateCredentialsRequest();
    
            body.credentials = (Dictionary<string, object>)credentials;
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "patch",
                string.Format($"/service/platform/assets/v1.0/credentials/{pluginId}"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddCredentialsResponse>(response["content"].ToString());
        }
    
        public AddCredentialsResponse? updateCredentials(
            string pluginId,
            Dictionary<string, object> credentials
        )
        {
            return Task.Run(() => updateCredentialsAsync(
            pluginId:pluginId,
            credentials:credentials)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Delete credentials of a transformation module.
        /// </summary>
        /// <remarks>
        /// Delete credentials of a transformation module, for an organization. 
        /// </remarks>
        /// <param name="pluginId">ID of the plugin whose credentials are being deleted</param>
        public async Task<AddCredentialsResponse?> deleteCredentialsAsync(
            string pluginId
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("pluginId", pluginId);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "delete",
                string.Format($"/service/platform/assets/v1.0/credentials/{pluginId}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddCredentialsResponse>(response["content"].ToString());
        }
    
        public AddCredentialsResponse? deleteCredentials(
            string pluginId
        )
        {
            return Task.Run(() => deleteCredentialsAsync(
            pluginId:pluginId)).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Add a preset.
        /// </summary>
        /// <remarks>
        /// Add a preset for an organization. 
        /// </remarks>
        /// <param name="presetName">Name of the preset</param>
        /// <param name="transformation">A chain of transformations, separated by `~`</param>
        /// <param name="params">Parameters object for transformation variables</param>
        public async Task<AddPresetResponse?> addPresetAsync(
            string presetName, 
            string transformation, 
            Dictionary<string, object>? @params = null
        )
        { 
    
            // Body
            var body = new AddPresetRequest();
    
            body.presetName = (string)presetName;body.transformation = (string)transformation;
    
            if (@params != null)
            { 
                body.@params = (Dictionary<string, object>)@params;
            }
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "post",
                string.Format($"/service/platform/assets/v1.0/presets"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddPresetResponse>(response["content"].ToString());
        }
    
        public AddPresetResponse? addPreset(
            string presetName, 
            string transformation, 
            Dictionary<string, object>? @params = null
        )
        {
            return Task.Run(() => addPresetAsync(
            presetName:presetName, 
            transformation:transformation, 
            @params:@params)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get all presets.
        /// </summary>
        /// <remarks>
        /// Get all presets of an organization. 
        /// </remarks>
        public async Task<AddPresetResponse?> getPresetsAsync()
        { 
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/presets"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddPresetResponse>(response["content"].ToString());
        }
    
        public AddPresetResponse? getPresets()
        {
            return Task.Run(() => getPresetsAsync()).GetAwaiter().GetResult();
        }
    
     
        /// <summary>
        /// Update a preset.
        /// </summary>
        /// <remarks>
        /// Update a preset of an organization. 
        /// </remarks>
        /// <param name="presetName">Name of the preset to be updated</param>
        /// <param name="archived">Indicates if the preset has been archived</param>
        public async Task<AddPresetResponse?> updatePresetAsync(
            string presetName,
            bool archived
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("presetName", presetName);
    
            // Body
            var body = new UpdatePresetRequest();
    
            body.archived = (bool)archived;
    
            // Body Validation
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));
    
            var response = await ApiClient.Execute(
                _configuration,
                "patch",
                string.Format($"/service/platform/assets/v1.0/presets/{presetName}"),
                null,
                body,
                "application/json"
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddPresetResponse>(response["content"].ToString());
        }
    
        public AddPresetResponse? updatePreset(
            string presetName,
            bool archived
        )
        {
            return Task.Run(() => updatePresetAsync(
            presetName:presetName,
            archived:archived)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Delete a preset.
        /// </summary>
        /// <remarks>
        /// Delete a preset of an organization. 
        /// </remarks>
        /// <param name="presetName">Name of the preset to be deleted</param>
        public async Task<AddPresetResponse?> deletePresetAsync(
            string presetName
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("presetName", presetName);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "delete",
                string.Format($"/service/platform/assets/v1.0/presets/{presetName}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddPresetResponse>(response["content"].ToString());
        }
    
        public AddPresetResponse? deletePreset(
            string presetName
        )
        {
            return Task.Run(() => deletePresetAsync(
            presetName:presetName)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get a preset.
        /// </summary>
        /// <remarks>
        /// Get a preset of an organization. 
        /// </remarks>
        /// <param name="presetName">Name of the preset to be fetched</param>
        public async Task<AddPresetResponse?> getPresetAsync(
            string presetName
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("presetName", presetName);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/presets/{presetName}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AddPresetResponse>(response["content"].ToString());
        }
    
        public AddPresetResponse? getPreset(
            string presetName
        )
        {
            return Task.Run(() => getPresetAsync(
            presetName:presetName)).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get default asset for playground
        /// </summary>
        /// <remarks>
        /// Get default asset for playground
        /// </remarks>
        public async Task<UploadResponse?> getDefaultAssetForPlaygroundAsync()
        { 
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/playground/default"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<UploadResponse>(response["content"].ToString());
        }
    
        public UploadResponse? getDefaultAssetForPlayground()
        {
            return Task.Run(() => getDefaultAssetForPlaygroundAsync()).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get all transformation modules
        /// </summary>
        /// <remarks>
        /// Get all transformation modules. 
        /// </remarks>
        public async Task<TransformationModulesResponse?> getModulesAsync()
        { 
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/playground/plugins"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<TransformationModulesResponse>(response["content"].ToString());
        }
    
        public TransformationModulesResponse? getModules()
        {
            return Task.Run(() => getModulesAsync()).GetAwaiter().GetResult();
        }
    
    
        /// <summary>
        /// Get Transformation Module by module identifier
        /// </summary>
        /// <remarks>
        /// Get Transformation Module by module identifier 
        /// </remarks>
        /// <param name="identifier">identifier of Transformation Module</param>
        public async Task<TransformationModuleResponse?> getModuleAsync(
            string identifier
        )
        { 
            // Payload
            var payload = new Dictionary<string, object>();
    
            payload.Add("identifier", identifier);
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/assets/v1.0/playground/plugins/{identifier}"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<TransformationModuleResponse>(response["content"].ToString());
        }
    
        public TransformationModuleResponse? getModule(
            string identifier
        )
        {
            return Task.Run(() => getModuleAsync(
            identifier:identifier)).GetAwaiter().GetResult();
        }
    }
    
    public class Organization
    {
        private readonly PixelbinConfig _configuration;
    
        public Organization(PixelbinConfig configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Get App Details
        /// </summary>
        /// <remarks>
        /// Get App and org details
        /// </remarks>
        public async Task<AppOrgDetails?> getAppOrgDetailsAsync()
        { 
    
            var response = await ApiClient.Execute<object>(
                _configuration,
                "get",
                string.Format($"/service/platform/organization/v1.0/apps/info"),
                null,
                null,
                ""
            );
    
            if ((int)response["status_code"] != 200)
            {
                throw new PixelbinServerResponseError(response["content"].ToString());
            }
            return JsonConvert.DeserializeObject<AppOrgDetails>(response["content"].ToString());
        }
    
        public AppOrgDetails? getAppOrgDetails()
        {
            return Task.Run(() => getAppOrgDetailsAsync()).GetAwaiter().GetResult();
        }
    }
}
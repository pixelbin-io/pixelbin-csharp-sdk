// Class Validators

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pixelbin.Platform.Models;

namespace Pixelbin.Platform.Models.Validators
{
    internal class AssetsValidator
    {
        public class fileUpload
        {
            // No properties required
        }
        
        public class urlUpload
        {
            // No properties required
        }
        
        public class createSignedUrl
        {
            // No properties required
        }
        
        public class listFiles
        {
            public string? name { get; set; }
            public string? path { get; set; }
            public string? format { get; set; }
            public List<string>? tags { get; set; }
            public bool? onlyFiles { get; set; }
            public bool? onlyFolders { get; set; }
            public int? pageNo { get; set; }
            public int? pageSize { get; set; }
            public string? sort { get; set; }
        }
        
        public class getFileById
        {
            public string? _id { get; set; }
        }
        
        public class getFileByFileId
        {
            public string? fileId { get; set; }
        }
        
        public class updateFile
        {
            public string? fileId { get; set; }
        }
        
        public class deleteFile
        {
            public string? fileId { get; set; }
        }
        
        public class deleteFiles
        {
            // No properties required
        }
        
        public class createFolder
        {
            // No properties required
        }
        
        public class getFolderDetails
        {
            public string? path { get; set; }
            public string? name { get; set; }
        }
        
        public class updateFolder
        {
            public string? folderId { get; set; }
        }
        
        public class deleteFolder
        {
            public string? _id { get; set; }
        }
        
        public class getFolderAncestors
        {
            public string? _id { get; set; }
        }
        
        public class addCredentials
        {
            // No properties required
        }
        
        public class updateCredentials
        {
            public string? pluginId { get; set; }
        }
        
        public class deleteCredentials
        {
            public string? pluginId { get; set; }
        }
        
        public class addPreset
        {
            // No properties required
        }
        
        public class getPresets
        {
            // No properties required
        }
        
        public class updatePreset
        {
            public string? presetName { get; set; }
        }
        
        public class deletePreset
        {
            public string? presetName { get; set; }
        }
        
        public class getPreset
        {
            public string? presetName { get; set; }
        }
        
        public class getDefaultAssetForPlayground
        {
            // No properties required
        }
        
        public class getModules
        {
            // No properties required
        }
        
        public class getModule
        {
            public string? identifier { get; set; }
        }
    }
}
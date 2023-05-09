// Platform Models.
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Pixelbin.Platform.Enums;

namespace Pixelbin.Platform.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    internal class UrlUploadRequest
    {
        
        public string url { get; set; }
        public string? path { get; set; }
        public string? name { get; set; }
        public AccessEnum? access { get; set; }
        public List<string>? tags { get; set; }
        public Dictionary<string, object>? metadata { get; set; }
        public bool? overwrite { get; set; }
        public bool? filenameOverride { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
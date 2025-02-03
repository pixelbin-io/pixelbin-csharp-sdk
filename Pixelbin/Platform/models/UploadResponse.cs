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
    public class UploadResponse
    {
        
        public string? _id { get; set; }
        public string? fileId { get; set; }
        public string? name { get; set; }
        public string? path { get; set; }
        public string? format { get; set; }
        public int? size { get; set; }
        public AccessEnum? access { get; set; }
        public List<string>? tags { get; set; }
        public Dictionary<string, object>? metadata { get; set; }
        public string? url { get; set; }
        public string? thumbnail { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
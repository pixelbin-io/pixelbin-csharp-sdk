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
    public class Credentials
    {
        
        public string? _id { get; set; }
        public string? createdAt { get; set; }
        public string? updatedAt { get; set; }
        public bool? isActive { get; set; }
        public string? orgId { get; set; }
        public string? pluginId { get; set; }
        public Dictionary<string, object>? credentials { get; set; }
        public Dictionary<string, object>? description { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
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
    public class AppSchema
    {
        
        public int? _id { get; set; }
        public int? orgId { get; set; }
        public string? name { get; set; }
        public string? token { get; set; }
        public List<string>? permissions { get; set; }
        public bool? active { get; set; }
        public string? createdAt { get; set; }
        public string? updatedAt { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
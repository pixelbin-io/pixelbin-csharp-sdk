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
    public class TransformationModuleResponse
    {
        
        public string? identifier { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public Dictionary<string, object>? credentials { get; set; }
        public List<Dictionary<string, object>>? operations { get; set; }
        public bool? enabled { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
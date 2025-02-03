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
    public class PresignedUrlV2
    {
        
        public string? url { get; set; }
        public Dictionary<string, object>? fields { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
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
    public class AddPresetResponse
    {
        
        public string presetName { get; set; }
        public string transformation { get; set; }
        public Dictionary<string, object>? @params { get; set; }
        public bool? archived { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
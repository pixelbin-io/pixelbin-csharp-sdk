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
    internal class CreateFolderRequest
    {
        
        public string? name { get; set; }
        public string? path { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
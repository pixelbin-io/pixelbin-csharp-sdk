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
    internal class UpdateCredentialsRequest
    {
        
        public Dictionary<string, object>? credentials { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
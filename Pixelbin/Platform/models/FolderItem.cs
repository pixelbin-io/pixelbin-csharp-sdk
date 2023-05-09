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
    public class FolderItem
    {
        
        public string _id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string type { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}
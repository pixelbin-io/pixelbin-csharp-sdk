using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pixelbin.Platform
{
    public class Enums
    {
        [JsonConverter(typeof(AccessEnumConverter))]
        public enum AccessEnum
        {
            
            PUBLIC_READ, 
            
            PRIVATE
        }
        
        internal class AccessEnumConverter : StringEnumConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(AccessEnum);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                if (reader.Value != null && reader.TokenType == JsonToken.String)
                {
                    string enumString = reader.Value.ToString().ToUpper().Replace('-', '_');
                    if (Enum.TryParse(typeof(AccessEnum), enumString, true, out var enumValue))
                    {
                        return enumValue;
                    }
                }

                throw new JsonSerializationException($"Unable to convert '{reader.Value}' to {typeof(AccessEnum)}.");
            }

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            {
                if (value is AccessEnum enumValue)
                {
                    switch (enumValue)
                    {
                        
                        case AccessEnum.PUBLIC_READ:
                            writer.WriteValue("public-read");
                            break;
                        
                        case AccessEnum.PRIVATE:
                            writer.WriteValue("private");
                            break;
                        
                        default:
                            writer.WriteNull();
                            break;
                    }
                }
            }
        }
    }

    public static class AccessEnumExtensions
    {
        public static string EnumValue(this Enums.AccessEnum e) {
            switch (e) {
                case Enums.AccessEnum.PUBLIC_READ:
                    return "public-read";
                case Enums.AccessEnum.PRIVATE:
                    return "private";
                
            }
            return "unknown enum value";
        }
    }
}

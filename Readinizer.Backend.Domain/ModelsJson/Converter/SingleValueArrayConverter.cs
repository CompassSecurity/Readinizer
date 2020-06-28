using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.Converter
{
    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var returnValue = new object();
            if (reader.TokenType == JsonToken.StartObject)
            {
                var instance = (T)serializer.Deserialize(reader, typeof(T));
                returnValue = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                returnValue = serializer.Deserialize(reader, objectType);
            }
            return returnValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}

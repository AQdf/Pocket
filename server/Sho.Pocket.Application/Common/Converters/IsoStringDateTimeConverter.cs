using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sho.Pocket.Application.Common.Converters
{
    public class IsoStringDateTimeConverter : DateTimeConverterBase
    {
        private const string DATE_ISO_STRING_FORMAT = "yyyy-MM-dd";

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception($"Unexpected token parsing date. Expected string, got {reader.TokenType}.");
            }

            string value = reader.Value as string;

            bool isSuccess = DateTime.TryParse(value, out DateTime result);

            if (!isSuccess)
            {
                throw new Exception($"Could not convert string '{value}' to date.");
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string dateIsoString;

            if (value is DateTime)
            {
                dateIsoString = ((DateTime)value).ToString(DATE_ISO_STRING_FORMAT);
            }
            else
            {
                throw new Exception("Expected date object value.");
            }

            writer.WriteValue(dateIsoString);
        }
    }
}

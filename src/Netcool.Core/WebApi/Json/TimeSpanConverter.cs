using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Netcool.Core.WebApi.Json
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        private readonly Regex _timeRegex = new Regex("^\\d{2}:\\d{2}:\\d{2}$");

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (_timeRegex.IsMatch(value))
            {
                return TimeSpan.Parse(value);
            }
            else
            {
                var datetime = DateTime.Parse(reader.GetString());
                return new TimeSpan(datetime.Hour, datetime.Minute, datetime.Second);
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}

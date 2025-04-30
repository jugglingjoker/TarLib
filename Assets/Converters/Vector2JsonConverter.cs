using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace TarLib.Assets.Converters {
    public class Vector2JsonConverter : JsonConverter<Vector2> {

        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var response = new Vector2();

            if (reader.TokenType != JsonTokenType.StartObject) {
                throw new JsonException();
            }

            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.EndObject) {
                    reader.Read();
                    return response;
                }
                if(reader.TokenType == JsonTokenType.PropertyName) {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch(propertyName) {
                        case "X":
                            response.X = (float)reader.GetDecimal();
                            break;
                        case "Y":
                            response.Y = (float)reader.GetDecimal();
                            break;
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteEndObject();
        }
    }
}

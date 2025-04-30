using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TarLib.Primitives;

namespace TarLib.Assets.Converters {
    public class RectanglePrimitiveJsonConverter : JsonConverter<RectanglePrimitive> {
        public override RectanglePrimitive Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var rectangle = new RectanglePrimitive();

            if(reader.TokenType != JsonTokenType.StartObject) {
                throw new JsonException();
            }
            while(reader.Read()) {
                if(reader.TokenType == JsonTokenType.EndObject) {
                    reader.Read();
                    return rectangle;
                }
                if(reader.TokenType == JsonTokenType.PropertyName) {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch(propertyName) {
                        case "X":
                            rectangle.X = (int)reader.GetDouble();
                            break;
                        case "Y":
                            rectangle.Y = (int)reader.GetDouble();
                            break;
                        case "Width":
                            rectangle.Width = (int)reader.GetDouble();
                            break;
                        case "Height":
                            rectangle.Height = (int)reader.GetDouble();
                            break;
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, RectanglePrimitive value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteNumber("Width", value.Width);
            writer.WriteNumber("Height", value.Height);
            writer.WriteEndObject();
        }
    }
}

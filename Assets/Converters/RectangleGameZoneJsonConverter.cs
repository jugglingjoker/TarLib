using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TarLib.Entities;
using TarLib.Primitives;

namespace TarLib.Assets.Converters {
    public class RectangleGameZoneJsonConverter : JsonConverter<RectangleGameZone> {
        public override RectangleGameZone Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            RectangleGameZone gameZone;
            RectanglePrimitive area = default;
            Vector2? manualPosition = default;
            RectangleGameZone.StickyLocation? location = default;

            if(reader.TokenType != JsonTokenType.StartObject) {
                throw new JsonException();
            }
            while(reader.Read()) {
                if(reader.TokenType == JsonTokenType.EndObject) {
                    reader.Read();
                    gameZone = new RectangleGameZone(area, manualPosition, location);
                    return gameZone;
                }
                if(reader.TokenType == JsonTokenType.PropertyName) {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch(propertyName) {
                        case "Area":
                            area = JsonSerializer.Deserialize<RectanglePrimitive>(ref reader, options);
                            break;
                        case "ManualPosition":
                            manualPosition = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                            break;
                        case "Location":
                            location = JsonSerializer.Deserialize<RectangleGameZone.StickyLocation>(ref reader, options);
                            break;
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, RectangleGameZone value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WritePropertyName("Area");
            JsonSerializer.Serialize(writer, value.Area, options);

            if(value.ManualPosition != default) {
                writer.WritePropertyName("ManualPosition");
                JsonSerializer.Serialize(writer, value.ManualPosition, options);
            } else {
                writer.WritePropertyName("Location");
                JsonSerializer.Serialize(writer, value.Location, options);
            }
            
            writer.WriteEndObject();
        }
    }
}

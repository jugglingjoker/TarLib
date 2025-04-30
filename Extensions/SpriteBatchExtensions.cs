using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.Entities.Drawable;
using TarLib.Graphics;
using TarLib.Primitives;

namespace TarLib.Extensions {
    public static class SpriteBatchExtensions {

        public static void Draw(this SpriteBatch spriteBatch, IDrawableTexture entity, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            if(entity.DrawVisible) {
                spriteBatch.Draw(
                    texture: entity.DrawTexture,
                    position: entity.DrawPosition + position,
                    sourceRectangle: entity.DrawTextureFrame,
                    color: entity.DrawColor,
                    rotation: entity.DrawRotation,
                    origin: entity.DrawOrigin,
                    scale: entity.DrawScale,
                    effects: entity.DrawEffects,
                    layerDepth: (endDepth - startDepth) * entity.DrawDepth + startDepth);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, IDrawableString entity, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            if(entity.DrawVisible) {
                spriteBatch.DrawString(
                    spriteFont: entity.DrawFont,
                    text: entity.DrawText,
                    position: entity.DrawPosition + position,
                    color: entity.DrawColor,
                    rotation: entity.DrawRotation,
                    origin: entity.DrawOrigin,
                    scale: entity.DrawScale,
                    effects: entity.DrawEffects,
                    layerDepth: (endDepth - startDepth) * entity.DrawDepth + startDepth);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, IDrawableCircle circle, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            if(circle.CircleDrawVisible) {
                
                if (circle.CircleBorderColor.A > 0 && circle.CircleBorderWidth > 0) {
                    spriteBatch.Draw(
                        texture: circle.CircleDrawTexture,
                        position: circle.CircleDrawCenter + position,
                        sourceRectangle: default,
                        color: circle.CircleBorderColor,
                        rotation: 0,
                        origin: circle.CircleDrawTexture.GetCenter(),
                        scale: circle.CircleDrawRadius * 2 / circle.CircleDrawTexture.Bounds.Width,
                        effects: SpriteEffects.None,
                        layerDepth: (endDepth - startDepth) * circle.CircleBorderDrawDepth + startDepth);
                }

                spriteBatch.Draw(
                    texture: circle.CircleDrawTexture,
                    position: circle.CircleDrawCenter + position,
                    sourceRectangle: default,
                    color: circle.CircleColor,
                    rotation: 0,
                    origin: circle.CircleDrawTexture.GetCenter(),
                    scale: (circle.CircleDrawRadius - circle.CircleBorderWidth) * 2 / circle.CircleDrawTexture.Bounds.Width,
                    effects: SpriteEffects.None,
                    layerDepth: (endDepth - startDepth) * circle.CircleDrawDepth + startDepth);
            }
        }

        
        public static void Draw(this SpriteBatch spriteBatch, IDrawableBox box, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            if (box.BoxDrawVisible) {
                var boxSize = new Vector2(box.BoxDrawWidth, box.BoxDrawHeight);
                if(box.BoxBorderColor.A > 0 && box.BoxBorderSize.TotalHorizontal != 0 || box.BoxBorderSize.TotalVertical != 0) {
                    spriteBatch.Draw(
                        texture: box.BoxDrawTexture,
                        position: box.BoxDrawPosition + position,
                        sourceRectangle: default,
                        color: box.BoxBorderColor,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: boxSize,
                        effects: SpriteEffects.None,
                        layerDepth: (endDepth - startDepth) * box.BoxBorderDrawDepth + startDepth);
                }

                spriteBatch.Draw(
                    texture: box.BoxDrawTexture,
                    position: box.BoxDrawPosition + position + box.BoxBorderSize.Position,
                    sourceRectangle: default,
                    color: box.BoxColor,
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: boxSize - box.BoxBorderSize.Total,
                    effects: SpriteEffects.None,
                    layerDepth: (endDepth - startDepth) * box.BoxDrawDepth + startDepth);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, IDrawableLine line, Vector2 position = default, float startDepth = 0, float endDepth = 1) {
            if (line.LineDrawVisible) {
                var lineSegment = new LineSegmentPrimitive(line.LineStart, line.LineEnd);
                var lineSize = new Vector2(lineSegment.Length, line.LineThickness);
                spriteBatch.Draw(
                    texture: line.LineDrawTexture,
                    position: lineSegment.Center + position,
                    sourceRectangle: default,
                    color: line.LineColor,
                    rotation: lineSegment.Angle,
                    origin: line.LineDrawTexture.GetCenter(),
                    scale: lineSize,
                    effects: SpriteEffects.None,
                    layerDepth: (endDepth - startDepth) * line.LineDrawDepth + startDepth);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, SimpleStaticTextureMap textureMap, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float layerDepth) {
            // TODO: Determine which range to draw based on rectangle bounds
            for (int i = textureMap.MinQuadX; i <= textureMap.MaxQuadX; i++) {
                for (int j = textureMap.MinQuadY; j <= textureMap.MaxQuadY; j++) {
                    if (textureMap[i, j] != default) {
                        var cornerPosition = Vector2.Transform((new Vector2(i * textureMap.QuadrantSize, j * textureMap.QuadrantSize) - origin) * scale, Matrix.CreateRotationZ(rotation));
                        spriteBatch.Draw(
                            texture: textureMap[i, j],
                            position: cornerPosition + position,
                            sourceRectangle: null,
                            color: color,
                            rotation: rotation,
                            origin: Vector2.Zero,
                            scale: scale,
                            effects: SpriteEffects.None,
                            layerDepth: layerDepth);
                    }
                }
            }
        }

        // TODO: Give better definition
        public static void Draw(this SpriteBatch spriteBatch,
            Texture2DLayeredTileMap textureMap,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth) {

            for (int i = 0; i < textureMap.MapWidth; i++) {
                for (int j = 0; j < textureMap.MapHeight; j++) {
                    if (textureMap[i, j] != null) {
                        spriteBatch.Draw(
                            texture: textureMap[i, j],
                            position: position + new Vector2(textureMap.TextureChunkWidth * i, textureMap.TextureChunkWidth * j) * scale,
                            sourceRectangle: sourceRectangle,
                            color: color,
                            rotation: rotation,
                            origin: origin,
                            scale: scale,
                            effects: effects,
                            layerDepth: layerDepth);
                    }
                }
            }
        }

        // TODO: Give better definition
        public static void Draw(this SpriteBatch spriteBatch,
            Texture2DLayeredTile layeredTile,
            Vector2 position,
            Color color) {

            for (int i = 0; i < layeredTile.LayersCount; i++) {
                if (layeredTile.Type == Texture2DLayeredTileType.Texture) {
                    spriteBatch.Draw(
                        texture: layeredTile.BaseTexture,
                        position: position,
                        sourceRectangle: null,
                        color: color,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: Vector2.One,
                        effects: SpriteEffects.None,
                        layerDepth: 0);
                } else {
                    // top left
                    var topLeftOffset = 0;
                    if (layeredTile.TopLeftMatch && layeredTile.LeftMatch && layeredTile.TopMatch) {
                        topLeftOffset = 0;
                    } else if (!layeredTile.LeftMatch && !layeredTile.TopMatch) {
                        topLeftOffset = 1;
                    } else if (layeredTile.TopMatch) {
                        if (layeredTile.LeftMatch) {
                            topLeftOffset = 2;
                        } else {
                            topLeftOffset = 4;
                        }
                    } else if (layeredTile.LeftMatch) {
                        topLeftOffset = 3;
                    }

                    var topRightOffset = 0;
                    if (layeredTile.TopRightMatch && layeredTile.RightMatch && layeredTile.TopMatch) {
                        topRightOffset = 0;
                    } else if (!layeredTile.RightMatch && !layeredTile.TopMatch) {
                        topRightOffset = 1;
                    } else if (layeredTile.TopMatch) {
                        if (layeredTile.RightMatch) {
                            topRightOffset = 2;
                        } else {
                            topRightOffset = 4;
                        }
                    } else if (layeredTile.RightMatch) {
                        topRightOffset = 3;
                    }

                    var bottomLeftOffset = 0;
                    if (layeredTile.BottomLeftMatch && layeredTile.LeftMatch && layeredTile.BottomMatch) {
                        bottomLeftOffset = 0;
                        // all matches, use tile 1
                    } else if (!layeredTile.LeftMatch && !layeredTile.BottomMatch) {
                        bottomLeftOffset = 1;
                        // no matches, use tile 2
                    } else if (layeredTile.BottomMatch) {
                        if (layeredTile.LeftMatch) {
                            bottomLeftOffset = 2;
                        } else {
                            bottomLeftOffset = 4;
                        }
                    } else if (layeredTile.LeftMatch) {
                        bottomLeftOffset = 3;
                    }

                    var bottomRightOffset = 0;
                    if (layeredTile.BottomRightMatch && layeredTile.RightMatch && layeredTile.BottomMatch) {
                        bottomRightOffset = 0;
                        // all matches, use tile 1
                    } else if (!layeredTile.RightMatch && !layeredTile.BottomMatch) {
                        bottomRightOffset = 1;
                        // no matches, use tile 2
                    } else if (layeredTile.BottomMatch) {
                        if (layeredTile.RightMatch) {
                            bottomRightOffset = 2;
                        } else {
                            bottomRightOffset = 4;
                        }
                    } else if (layeredTile.RightMatch) {
                        bottomRightOffset = 3;
                    }


                    // top left
                    spriteBatch.Draw(
                        texture: layeredTile.BaseTexture,
                        position: position,
                        sourceRectangle: new Rectangle(0 + topLeftOffset * 16, 0, 8, 8),
                        color: color,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: Vector2.One,
                        effects: SpriteEffects.None,
                        layerDepth: 0);
                    // top right
                    spriteBatch.Draw(
                        texture: layeredTile.BaseTexture,
                        position: position + new Vector2(8, 0),
                        sourceRectangle: new Rectangle(8 + topRightOffset * 16, 0, 8, 8),
                        color: color,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: Vector2.One,
                        effects: SpriteEffects.None,
                        layerDepth: 0);
                    // top left
                    spriteBatch.Draw(
                        texture: layeredTile.BaseTexture,
                        position: position + new Vector2(0, 8),
                        sourceRectangle: new Rectangle(0 + bottomLeftOffset * 16, 8, 8, 8),
                        color: color,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: Vector2.One,
                        effects: SpriteEffects.None,
                        layerDepth: 0);
                    // top right
                    spriteBatch.Draw(
                        texture: layeredTile.BaseTexture,
                        position: position + new Vector2(8, 8),
                        sourceRectangle: new Rectangle(8 + bottomRightOffset * 16, 8, 8, 8),
                        color: color,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: Vector2.One,
                        effects: SpriteEffects.None,
                        layerDepth: 0);
                }
            }


        }
    }
}

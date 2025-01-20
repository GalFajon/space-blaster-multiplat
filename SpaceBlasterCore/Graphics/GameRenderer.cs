using SpaceBlaster.Scene;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using GameSpecific;
using Scene.Components;
using System.Reflection.Metadata;
namespace SpaceBlaster.Graphics;

public class GameRenderer : DrawableGameComponent
{

    private SpriteBatch _spriteBatch;
    private SpaceBlasterGame _game;

    public GameRenderer(SpaceBlasterGame game) : base(game)
    {
        _game = game;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    public override void Draw(GameTime gameTime)
    {
        if (_game._currentLevel.Camera != null)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix t = _game.getRendererTransformationMatrix();

            _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, t);

            foreach (object item in _game._currentLevel.Components)
            {
                if (item is Position itemWithPosition && item is IAnimatable animatable)
                {
                    animatable.AnimationPlayer.Update(gameTime);
                    AnimatedSprite sprite = animatable.AnimationPlayer.GetCurrentSprite();

                    if (sprite != null)
                    {
                        if (_game.isOnScreen(itemWithPosition.Pos, sprite.Width * sprite.Scale, sprite.Height * sprite.Scale))
                        {
                            _spriteBatch.Draw(sprite.texture, itemWithPosition.Pos, sprite.rects[sprite.currentFrame], sprite.color, sprite.Rotation, new Vector2(0, 0), sprite.Scale, sprite.effect, sprite.depth);
                        }
                    }
                }
            }

            _spriteBatch.End();
        }
    }
}
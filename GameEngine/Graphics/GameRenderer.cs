using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Scene.Components;
using SpaceBlaster;

namespace GameEngine.Graphics;

public class GameRenderer : DrawableGameComponent
{

    private SpriteBatch _spriteBatch;
    private GameBase _game;

    public GameRenderer(GameBase game) : base(game)
    {
        _game = game;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    public bool IsOnScreen(Vector2 pos, float width = 0, float height = 0)
    {
        var e1 = Vector2.Transform(new Vector2(0, 0), Matrix.Invert(GetRendererTransformationMatrix()));
        var e4 = new Vector2(_game.CurrentScene.Camera.Pos.X + GameBase.VirtualResolutionWidth / SpaceBlasterGame.Settings.CameraZoom, _game.CurrentScene.Camera.Pos.Y + GameBase.VirtualResolutionHeight / SpaceBlasterGame.Settings.CameraZoom);
        return
            pos.X + width > e1.X &&
            pos.X - width < e4.X &&
            pos.Y - height < e4.Y &&
            pos.Y + height > e1.Y;
    }

    public Matrix GetRendererTransformationMatrix()
    {
        Matrix t = Matrix.CreateTranslation(
            new Vector3(
                -_game.CurrentScene.Camera.Pos.X,
                -_game.CurrentScene.Camera.Pos.Y,
                0
            )
        )
        * Matrix.CreateRotationZ(_game.CurrentScene.Camera.Rot)
        * Matrix.CreateScale(new Vector3(_game.CurrentScene.Camera.ScaleX, _game.CurrentScene.Camera.ScaleY, 1.0f));

        return t;
    }

    public override void Draw(GameTime gameTime)
    {
        if (_game.CurrentScene.Camera != null)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix t = GetRendererTransformationMatrix();

            _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, t);

            foreach (SceneObject item in _game.CurrentScene.Components)
            {
                if (item is Position itemWithPosition)
                {
                    if (item is IAnimatable animatable)
                    {
                        animatable.AnimationPlayer.Update(gameTime);
                        AnimatedSprite sprite = animatable.AnimationPlayer.GetCurrentSprite();
                        
                        if (sprite != null)
                        {
                            if (IsOnScreen(itemWithPosition.Pos, sprite.Width * sprite.Scale, sprite.Height * sprite.Scale))
                            {
                                _spriteBatch.Draw(sprite.texture, itemWithPosition.Pos, sprite.rects[sprite.currentFrame], sprite.color * sprite.opacity, sprite.Rotation, sprite.center, sprite.Scale, sprite.effect, sprite.depth);
                            }
                        }
                    }
                    else if (item is GameEngine.Scene.Components.IDrawable drawable)
                    {
                        Sprite sprite = drawable.sprite;
                        if (sprite != null)
                        {
                            if (IsOnScreen(itemWithPosition.Pos, sprite.rect.Width * sprite.Scale, sprite.rect.Height * sprite.Scale))
                            {
                                _spriteBatch.Draw(sprite.texture, itemWithPosition.Pos, sprite.rect, sprite.color * sprite.opacity, sprite.Rotation, sprite.center, sprite.Scale, sprite.effect, sprite.depth);
                            }
                        }
                    }


                }
            }

            _spriteBatch.End();
        }
    }
}
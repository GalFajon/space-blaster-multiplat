using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using GameSpecific;
using GameEngine.Gameplay.Input;
using GameEngine.Scene.UI;
using GameEngine.Scene.Components;
using SpaceBlaster;

namespace GameEngine.Graphics;

public class UIRenderer : DrawableGameComponent
{

    private SpriteBatch _spriteBatch;
    private GameBase _game;
    private SpriteFont _font = null;

    public UIRenderer(GameBase game, InputManager input) : base(game)
    {
        _game = game;
        _font = Game.Content.Load<SpriteFont>("File");
    }

    public static Sprite ButtonSprite;
    public static Sprite ButtonClickedSprite;
    public static Sprite SmallButtonSprite;
    public static Sprite SmallButtonClickedSprite;

    public static int LabelScale = 1;
    public static int ButtonScale = 3;

    public static int ButtonWidth = 32;
    public static int ButtonHeight = 16;
    public static int ButtonSmallWidth = 16;
    public static int ButtonSmallHeight = 16;

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
       
        ButtonSprite = new Sprite(_game.Content.Load<Texture2D>("ui"), new Rectangle(0, 0, 32, 16), new Vector2(0, 0), 1);
        ButtonClickedSprite = new Sprite(_game.Content.Load<Texture2D>("ui"), new Rectangle(0, 17, 32, 16), new Vector2(0, 0), 1);
        SmallButtonSprite = new Sprite(_game.Content.Load<Texture2D>("ui"), new Rectangle(0, 34, 16, 16), new Vector2(0, 0), 1);
        SmallButtonClickedSprite = new Sprite(_game.Content.Load<Texture2D>("ui"), new Rectangle(0, 51, 16, 16), new Vector2(0, 0), 1);
    }

    public Matrix GetUIRendererTransformationMatrix()
    {
        Matrix t = Matrix.CreateRotationZ(_game.CurrentScene.Camera.Rot) *
            Matrix.CreateScale(new Vector3(((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GameBase.VirtualResolutionWidth), ((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / (float)GameBase.VirtualResolutionHeight), 1.0f));

        return t;
    }

    public override void Draw(GameTime gameTime)
    {
        Matrix t = GetUIRendererTransformationMatrix();
        _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, t);

        foreach (object item in _game.CurrentScene.Components)
        {
            if (item is Label l)
            {
                _spriteBatch.DrawString(_font, l.Text, l.Pos, l.Color, 0, new Vector2(0,0), l.Scale * LabelScale, SpriteEffects.None, 0);
            }
            else if (item is Position itemWithPosition)
            {
                Sprite sprite = null;
                AnimatedSprite asprite = null;

                if (item is Image i) sprite = i.Sprite;

                if (item is Button b)
                {
                    if (b.Big == true)
                    {
                        if (b.clicked == false) sprite = ButtonSprite;
                        else sprite = ButtonClickedSprite;
                    }
                    else
                    {
                        if (b.clicked == false) sprite = SmallButtonSprite;
                        else sprite = SmallButtonClickedSprite;
                    }
                }

                if (item is IUIAnimatable a)
                {
                    a.AnimationPlayer.Update(gameTime);
                    asprite = a.AnimationPlayer.GetCurrentSprite();
                }

                if (sprite != null)
                {
                    _spriteBatch.Draw(sprite.texture, itemWithPosition.Pos, sprite.rect, sprite.color * sprite.opacity, 0, new Vector2(0, 0), sprite.Scale * ButtonScale, sprite.effect, 0.1f);
                }

                if (asprite != null)
                {
                    _spriteBatch.Draw(asprite.texture, itemWithPosition.Pos, asprite.rects[asprite.currentFrame], asprite.color * asprite.opacity, asprite.Rotation, asprite.center, asprite.Scale, asprite.effect, asprite.depth);
                }
            }
        }

        _spriteBatch.End();
    }
}
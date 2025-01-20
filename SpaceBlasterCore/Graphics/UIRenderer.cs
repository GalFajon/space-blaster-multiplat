using SpaceBlaster.Scene;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using GameSpecific;
using Scene.Components;
namespace SpaceBlaster.Graphics;

public class UIRenderer : DrawableGameComponent
{

    private SpriteBatch _spriteBatch;
    private Dictionary<String, Sprite> _spriteBank = new Dictionary<string, Sprite>();
    private InputManager _input;
    private SpaceBlasterGame _game;
    private SpriteFont _font = null;

    public UIRenderer(SpaceBlasterGame game, InputManager input) : base(game)
    {
        _game = game;
        _input = input;
        _font = Game.Content.Load<SpriteFont>("File");
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // change sprite bank into a class that gets sprite based on the inserted SceneObject
        _spriteBank.Add("button", new Sprite(_game.Content.Load<Texture2D>("atlas"), new Rectangle(3, 57, 32, 16), new Vector2(0, 0), 3));
        _spriteBank.Add("button_clicked", new Sprite(_game.Content.Load<Texture2D>("atlas"), new Rectangle(3, 74, 32, 16), new Vector2(0, 0), 3));
        _spriteBank.Add("small_button", new Sprite(_game.Content.Load<Texture2D>("atlas"), new Rectangle(88, 34, 16, 16), new Vector2(0, 0), 3));
        _spriteBank.Add("small_button_clicked", new Sprite(_game.Content.Load<Texture2D>("atlas"), new Rectangle(88, 51, 16, 16), new Vector2(0, 0), 3));
    }

    public override void Draw(GameTime gameTime)
    {
        //GraphicsDevice.Clear(Color.Black);
        Matrix t = _game.getUIRendererTransformationMatrix();
        _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, t);

        foreach (object item in _game._currentLevel.Components)
        {
            if (item is Label l)
            {
                _spriteBatch.DrawString(_font, l.Text, l.Pos, l.Color);
            }
            else if (item is Position itemWithPosition)
            {
                Sprite sprite = null;

                if (item is Image i) sprite = i.Sprite;

                if (item is Button b)
                {
                    if (b.Big)
                    {
                        if (b.clicked == false) sprite = _spriteBank["button"];
                        else sprite = _spriteBank["button_clicked"];
                    }
                    else
                    {
                        if (b.clicked == false) sprite = _spriteBank["small_button"];
                        else sprite = _spriteBank["small_button_clicked"];
                    }
                }

                if (sprite != null)
                {
                    _spriteBatch.Draw(sprite.texture, itemWithPosition.Pos, sprite.rect, Color.White, 0, new Vector2(0, 0), sprite.Scale, sprite.effect, 0.1f);
                }
            }
        }

        _spriteBatch.End();
    }
}
using GameEngine.Gameplay.Input;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using GameEngine.Graphics;
using System;

namespace GameEngine.Scene.UI;

public class Button : Position, IInputHandler
{
    public Label label = null;
    private Rectangle rect;
    public bool clicked = false;
    private bool big = false;
    public bool Big {
        get {
            return big;
        }
        set {
            if (this.Big)
            {
                this.rect = new Rectangle(0, 0, UIRenderer.ButtonWidth * UIRenderer.ButtonScale, UIRenderer.ButtonHeight * UIRenderer.ButtonScale);
            }
            else
            {
                this.rect = new Rectangle(0, 0, UIRenderer.ButtonSmallWidth * UIRenderer.ButtonScale, UIRenderer.ButtonSmallHeight * UIRenderer.ButtonScale);
            }

            big = value;
        } 
    }

    public Button(int x, int y, Label l, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.Big = true;

        if (l != null)
        {
            label = l;
            l.Parent = this;
            scene.Spawn(label);
        }
    }

    public delegate void ClickHandler();
    public ClickHandler clickHandler = null;

    public void HandleClick() {
        if (clickHandler != null) this.clickHandler.Invoke();
    }

    public void HandleInput(GameTime gameTime, TouchCollection touchstate, KeyboardState keyboard, MouseState mouse, Matrix UITransform, Matrix RendererTransform)
    {
        if (OperatingSystem.IsWindows())
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (!this.clicked)
                {
                    var r = this.rect;
                    r.Offset(this.Pos.X, this.Pos.Y);
                    if (r.Contains(InputManager.MousePosUI.X, InputManager.MousePosUI.Y)) this.clicked = true;
                }
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                if (this.clicked)
                {
                    var r = this.rect;
                    r.Offset(this.Pos.X, this.Pos.Y);
                    if (r.Contains(InputManager.MousePosUI.X, InputManager.MousePosUI.Y)) this.HandleClick();
                }

                this.clicked = false;
            }
        }
        else if (OperatingSystem.IsAndroid())
        {
            foreach (var touch in touchstate)
            {
                var r = this.rect;
                r.Offset(this.Pos.X, this.Pos.Y);
                Vector2 UITouchPos = Vector2.Transform(new Vector2(touch.Position.X, touch.Position.Y), Matrix.Invert(UITransform));

                if (touch.State == TouchLocationState.Pressed)
                {
                    if (!this.clicked)
                    {
                        if (r.Contains(UITouchPos))
                        {
                            this.clicked = true;
                        }
                    }
                }
                else if (touch.State == TouchLocationState.Released)
                {
                    if (this.clicked)
                    {
                        this.HandleClick();
                        this.clicked = false;
                    }
                }
            }
        }
    }
}
/*
    Takes concrete inputs on a device and transforms them into "states" relevant to the game
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SpaceBlaster.Scene;
using GameSpecific;
using SpaceBlaster;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

public class InputManager : GameComponent
{
    // inputs
    public bool Up = false;
    public bool Down = false;
    public bool Left = false;
    public bool Right = false;
    public bool Restart = false;

    public bool Shoot = false;
    public bool SwitchWeapon = false;
    public bool SwitchingWeapon = false;
    public bool CanRestart = false;

#if ANDROID

    Vector2 MovePivot = new Vector2(0,0);
    bool Moving = false;
    Vector2 MoveDir = new Vector2(0,0);
    Vector2 ShootPivot = new Vector2(0,0);
    Vector2 WeaponDir = new Vector2(0, 0);
#endif

    private SpaceBlasterGame _game;
    private GraphicsDeviceManager _graphics;
    // bindings (TODO) -> allow players to manually rebind controls, add some default bindings as well

    // devices

    public InputManager(SpaceBlasterGame game, GraphicsDeviceManager graphics) : base(game)
    {
        _game = game;
        _graphics = graphics;
    }

    public override void Update(GameTime gameTime)
    {
        // touch
#if ANDROID
        TouchCollection t = TouchPanel.GetState();
#endif

#if WINDOWS
        // keyboard
        KeyboardState state = Keyboard.GetState();

        // move
        if (state.IsKeyDown(Keys.D)) Right = true;
        else Right = false;

        if (state.IsKeyDown(Keys.A)) Left = true;
        else Left = false;

        if (state.IsKeyDown(Keys.W)) Up = true;
        else Up = false;

        if (state.IsKeyDown(Keys.S)) Down = true;
        else Down = false;

        if (state.IsKeyDown(Keys.R)) Restart = true;
        else Restart = false;

        if (state.IsKeyDown(Keys.E) && !SwitchingWeapon)
        {
            SwitchWeapon = true;
            SwitchingWeapon = true;
        }
        else if (state.IsKeyUp(Keys.E)) SwitchingWeapon = false;
#endif

        // shoot
#if WINDOWS
        MouseState mouse = Mouse.GetState();
        //Vector2 scaledMousePos = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(_game.getRendererTransformationMatrix()));
        Vector2 UIMousePos = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(_game.getUIRendererTransformationMatrix()));

        if (mouse.LeftButton == ButtonState.Pressed) Shoot = true;
        else Shoot = false;
#endif

#if ANDROID
        foreach (var  touch in t)
        {
            Vector2 UITouchPos = Vector2.Transform(new Vector2(touch.Position.X, touch.Position.Y), Matrix.Invert(_game.getUIRendererTransformationMatrix()));

            if (UITouchPos.X < (SpaceBlasterGame.virtualResolutionWidth / 2))
            {
                if (touch.State == TouchLocationState.Pressed) {
                    MovePivot = UITouchPos;
                    Moving = true;
                }
                else if (touch.State == TouchLocationState.Moved)
                {
                    if (UITouchPos != MovePivot) {
                        MoveDir = Vector2.Subtract(UITouchPos, MovePivot);
                        MoveDir.Normalize();
                    }
                }
                else if (touch.State == TouchLocationState.Released) Moving = false;
            }

            if (UITouchPos.X > (SpaceBlasterGame.virtualResolutionWidth / 2))
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    ShootPivot = UITouchPos;
                    Shoot = true;
                }
                else if (touch.State == TouchLocationState.Moved)
                {
                    if (UITouchPos != ShootPivot) {
                        WeaponDir = Vector2.Subtract(UITouchPos, ShootPivot);
                        WeaponDir.Normalize();
                    }
                }
                else if (touch.State == TouchLocationState.Released) Shoot = false;
            }
        }
#endif

        if (Restart && CanRestart && _game._currentLevel is Level l)
        {
            l.Lose();
        }

        foreach (object item in _game._currentLevel.Components)
        {
            if (item is Player p)
            {
                if (p.Health <= 0)
                {
                    CanRestart = true;
                    p.Destroy();
                }

#if WINDOWS
                p.Move(Left, Up, Down, Right);
#endif
#if ANDROID
                if (Moving) {
                    Debug.WriteLine(MoveDir);
                    p.Move(MoveDir);
                }
                else {
                    MoveDir = new Vector2(0,0);
                    p.Move(MoveDir);
                }
#endif

                if (SwitchWeapon)
                {
                    p.SwitchWeapon();
                    SwitchWeapon = false;
                }

                if (p.currentWeapon != null)
                {
#if WINDOWS
                    Vector2 dir = Vector2.Subtract(new Vector2(UIMousePos.X, UIMousePos.Y), new Vector2(SpaceBlasterGame.virtualResolutionWidth / (2 * SpaceBlasterGame.Settings.CameraZoom), SpaceBlasterGame.virtualResolutionHeight / (2 * SpaceBlasterGame.Settings.CameraZoom)));
                    dir = Vector2.Normalize(dir);
                    p.currentWeapon.Aim(dir);
#endif
#if ANDROID
                    p.currentWeapon.Aim(WeaponDir);

                    if (Shoot && WeaponDir.X != 0 && WeaponDir.Y != 0)
                    {
                        p.currentWeapon.Shoot();
                    }
#endif
#if WINDOWS
                    if (Shoot && dir.X != 0 && dir.Y != 0)
                    {
                        p.currentWeapon.Shoot();
                    }
#endif
                }
            }

            // ui
            if (item is Button b)
            {
#if ANDROID
                foreach (var  touch in t)
                {
                    if (touch.State == TouchLocationState.Pressed)
                    {
                        if (!b.clicked)
                        {
                            var r = b.rect;
                            r.Offset(b.Pos.X, b.Pos.Y);
                            Vector2 UITouchPos = Vector2.Transform(new Vector2(touch.Position.X, touch.Position.Y), Matrix.Invert(_game.getUIRendererTransformationMatrix()));
                            
                            Debug.WriteLine(r.Contains(UITouchPos.X, UITouchPos.Y));

                            if (r.Contains(UITouchPos.X, UITouchPos.Y))
                            {
                                b.clicked = true;
                                break;
                            }
                        }
                    }
                    else if (touch.State == TouchLocationState.Released)
                    {
                        if (b.clicked)
                        {
                            var r = b.rect;
                            r.Offset(b.Pos.X, b.Pos.Y);
                            Vector2 UITouchPos = Vector2.Transform(new Vector2(touch.Position.X, touch.Position.Y), Matrix.Invert(_game.getUIRendererTransformationMatrix()));

                            if (r.Contains(UITouchPos.X, UITouchPos.Y))
                            {
                                b.HandleClick();
                                break;
                            }
                        }

                        b.clicked = false;
                    }
                }
#endif

#if WINDOWS
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (!b.clicked)
                    {
                        var r = b.rect;
                        r.Offset(b.Pos.X, b.Pos.Y);
                        if (r.Contains(UIMousePos.X, UIMousePos.Y)) b.clicked = true;
                    }
                }
                else if (mouse.LeftButton == ButtonState.Released)
                {
                    if (b.clicked)
                    {
                        var r = b.rect;
                        r.Offset(b.Pos.X, b.Pos.Y);
                        if (r.Contains(UIMousePos.X, UIMousePos.Y)) b.HandleClick();
                    }

                    b.clicked = false;
                }
#endif
            }
        }

        base.Update(gameTime);
    }
}
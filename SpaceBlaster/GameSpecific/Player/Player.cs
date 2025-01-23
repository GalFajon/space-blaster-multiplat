using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using SpaceBlaster;
using System.Collections.Generic;
using System.Diagnostics;
using GameEngine.Gameplay.Input;
using System;
namespace GameSpecific;

public class Player : Physics, IAnimatable, IInputHandler
{
    private float health = 3;
    public float MaxHealth = 3;
    public float Health
    {
        get => health;
        set
        {
            if (value <= MaxHealth && value >= 0) health = value;

            if (health == 3) healthBar.SetState(BarState.HIGH);
            else if (health == 2) healthBar.SetState(BarState.MEDIUM);
            else healthBar.SetState(BarState.LOW);

            if (health <= 0)
            {
                //this.Destroy();

                var e = new Explosion(this.Pos.X - this.Rect.Center.X, this.Pos.Y - this.Rect.Center.Y, this.scene);
                this.scene.Spawn(e);

                Button restart = new Button(
                    SpaceBlasterGame.VirtualResolutionWidth / 2 - 8 * 3 - 16 * 3,
                    SpaceBlasterGame.VirtualResolutionHeight / 2 - 16 * 2,
                    new Label(16, 10, "Restart", Color.White, this.scene, null),
                    this.scene,
                    null
                );

                restart.clickHandler = delegate ()
                {
                    ((Level)this.scene).Lose();
                };

                this.scene.Spawn(restart);
                this.Destroy();

                //if (this.scene is GameSpecific.Level l) l.Lose();
            }
        }
    }

    public readonly float MaxSpeed = 180;

    private Bar healthBar = new Bar(0, -20, null);
    private PlayerMoveMarker moveMarker;
    public WeaponStats primaryWeapon = PremadeWeapons.Pistol;
    public WeaponStats secondaryWeapon = PremadeWeapons.Shotgun;
    public bool whichWeapon = false;
    public Weapon currentWeapon = null;
    private bool canSwitchWeapon = true;
    public AnimationPlayer AnimationPlayer { get; set; }

    // Android controls
    Vector2 MovePivot = new Vector2(0, 0);
    Vector2 AimPivot = new Vector2(0, 0);

    public Player(WeaponStats primary, WeaponStats secondary, float x, float y, Scene scene, SceneObject parent = null, int color = 3) : base(x, y, new Rectangle(0, 0, 32, 32), true, scene, parent)
    {
        this.primaryWeapon = primary;
        this.secondaryWeapon = secondary;

        var still = new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(0, 0), 2, 0.2, 16, 16, 3, 0.05f);
        var walk = new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(0, 16), 2, 0.2, 16, 16, 3, 0.05f);

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, still},
                { 1, walk }
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Pos = new Vector2(x, y);

        healthBar.Parent = this;
        this.scene.Spawn(healthBar);
        healthBar.SetState(BarState.HIGH);

        currentWeapon = new Weapon(primaryWeapon, 8 * 3, 8 * 3, this.scene, this);
        currentWeapon.Parent = this;
        this.scene.Spawn(currentWeapon);

        if (OperatingSystem.IsAndroid())
        {
            this.moveMarker = new PlayerMoveMarker(0, 0, this.scene, this);
            this.scene.Spawn(moveMarker);
        }
    }

    public void SwitchWeapon()
    {
        if (whichWeapon) currentWeapon.stats = primaryWeapon;
        else currentWeapon.stats = secondaryWeapon;

        whichWeapon = !whichWeapon;
    }

    // this could just take the direction vector, no need for bools
    public void Move(bool left, bool up, bool down, bool right)
    {
        Vector2 dir = new Vector2(0, 0);

        if (left) dir.X += -1;
        else if (right) dir.X += 1;

        if (up) dir.Y += -1;
        else if (down) dir.Y += 1;

        if (left || right || up || down) this.AnimationPlayer.SetCurrentAnimation(1);
        else this.AnimationPlayer.SetCurrentAnimation(0);

        var spr = this.AnimationPlayer.GetCurrentSprite();
        if (this.Direction.X < 0) spr.FlipH();
        else spr.Unflip();

        this.Speed = this.MaxSpeed;
        this.Direction = dir;
    }

    public void Move(Vector2 dir)
    {
        if (dir.X != 0 || dir.Y != 0) this.AnimationPlayer.SetCurrentAnimation(1);
        else this.AnimationPlayer.SetCurrentAnimation(0);

        var spr = this.AnimationPlayer.GetCurrentSprite();
        if (this.Direction.X < 0) spr.FlipH();
        else spr.Unflip();

        this.Speed = this.MaxSpeed;
        this.Direction = dir;
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is Projectile) this.Health -= 1;
        else if (collided is PlayerProjectile) return;
        else if (collided is EntranceDoor) this.currentWeapon.CanShoot = false;
    }

    public void HandleInput(GameTime gameTime, TouchCollection touchstate, KeyboardState keyboard, MouseState mouse, Matrix renderer, Matrix UI)
    {
        if (OperatingSystem.IsWindows())
        {
            this.Move(keyboard.IsKeyDown(Keys.A), keyboard.IsKeyDown(Keys.W), keyboard.IsKeyDown(Keys.S), keyboard.IsKeyDown(Keys.D));

            if (keyboard.IsKeyDown(Keys.E)) canSwitchWeapon = true;
            if (keyboard.IsKeyUp(Keys.E) && canSwitchWeapon)
            {
                this.SwitchWeapon();
                canSwitchWeapon = false;
            }

            if (this.currentWeapon != null)
            {
                Vector2 dir = Vector2.Subtract(new Vector2(InputManager.MousePosUI.X, InputManager.MousePosUI.Y), new Vector2(SpaceBlasterGame.VirtualResolutionWidth / (2 * SpaceBlasterGame.Settings.CameraZoom), SpaceBlasterGame.VirtualResolutionHeight / (2 * SpaceBlasterGame.Settings.CameraZoom)));
                dir = Vector2.Normalize(dir);

                this.currentWeapon.Aim(dir);

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    this.currentWeapon.Shoot();
                }
            }
        }
        else if (OperatingSystem.IsAndroid())
        {
            foreach (var touch in touchstate)
            {
                Vector2 UITouchPos = Vector2.Transform(new Vector2(touch.Position.X, touch.Position.Y), Matrix.Invert(renderer));
    
                if (UITouchPos.X < SpaceBlasterGame.VirtualResolutionWidth / 2)
                {
                    if (touch.State == TouchLocationState.Pressed) MovePivot = UITouchPos;
                    else if (touch.State == TouchLocationState.Moved && MovePivot.X != 0 && MovePivot.Y != 0)
                    {
                        if (MovePivot != UITouchPos)
                        {
                            var moveDir = Vector2.Subtract(UITouchPos, MovePivot);
                            moveDir.Normalize();
                            if (moveDir.X != 0 && moveDir.Y != 0)
                            {
                                this.Move(moveDir);
                                this.moveMarker.Aim(moveDir);
                            }
                        }
                    }
                    else if (touch.State == TouchLocationState.Released) this.Move(Vector2.Zero);
                }
                else if (UITouchPos.X > SpaceBlasterGame.VirtualResolutionWidth / 2)
                {
                    if (touch.State == TouchLocationState.Pressed) AimPivot = UITouchPos;
                    else if (touch.State == TouchLocationState.Moved && AimPivot.X != 0 && AimPivot.Y != 0)
                    {
                        if (AimPivot != UITouchPos) { 
                            var aimDir = Vector2.Subtract(UITouchPos, AimPivot);
                            aimDir.Normalize();
                            this.currentWeapon.Aim(aimDir);
                            this.currentWeapon.Shoot();
                        }
                    }
                }
            }
        }
    }

}
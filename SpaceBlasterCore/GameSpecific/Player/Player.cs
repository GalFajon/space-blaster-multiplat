using Microsoft.Xna.Framework;
namespace GameSpecific;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Scene.Components;
using SpaceBlaster;
using SpaceBlaster.Scene;

public class Player : Physics, IAnimatable
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

#if  ANDROID
                Button restart = new Button(
                    SpaceBlasterGame.virtualResolutionWidth / 2 - 8 * 3 - 16 * 3,
                    SpaceBlasterGame.virtualResolutionHeight / 2 - 16 * 2,
                    32 * 3,
                    16 * 3,
                    null,
                    new Label(16, 10, "Restart", Color.White, this.scene, null),
                    this.scene,
                    null
                );

                restart.clickHandler = delegate ()
                {
                    ((Level)this.scene).Lose();
                };

                this.scene.Spawn(restart);
#endif
                //if (this.scene is GameSpecific.Level l) l.Lose();
            }
        }
    }

    public readonly float MaxSpeed = 180;

    private Bar healthBar = new Bar(0, -20, null);

    public WeaponStats primaryWeapon = PremadeWeapons.Pistol;
    public WeaponStats secondaryWeapon = PremadeWeapons.Shotgun;
    public bool whichWeapon = false;
    public Weapon currentWeapon = null;

    public AnimationPlayer AnimationPlayer { get; set; }

    public Player(WeaponStats primary, WeaponStats secondary, float x, float y, SpaceBlaster.Scene.Scene scene, SceneObject parent = null, int color = 3) : base(x, y, new Rectangle(0, 0, 32, 32), true, scene, parent)
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

}
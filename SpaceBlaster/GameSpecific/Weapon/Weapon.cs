using Microsoft.Xna.Framework;
namespace GameSpecific;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using SpaceBlaster;

public enum WeaponType
{
    GUN,
    FLAMETHROWER
}

public struct WeaponStats
{
    public WeaponStats(int projectileCount, float damage, float spread, double firerate, int speed, string displayName, WeaponType type = WeaponType.GUN, bool unlocked = true, int price = 0)
    {
        this.ProjectileCount = projectileCount;
        this.Damage = damage;
        this.Spread = spread;
        this.FireRate = firerate;
        this.ProjectileSpeed = speed;
        this.Price = price;
        this.DisplayName = displayName;
        this.Unlocked = unlocked;
        this.type = type;
    }
    public int ProjectileCount;
    public float Damage;
    public float Spread;
    public double FireRate;
    public int ProjectileSpeed;
    public int Price = 0;
    public bool Unlocked;
    public string DisplayName = "";
    public WeaponType type = WeaponType.GUN;
}

public struct PremadeWeapons
{
    //player weapons
    public static WeaponStats Pistol = new WeaponStats(1, 1, 0.0f, 600, 600, "Pistol", WeaponType.GUN, SpaceBlasterGame.Settings.PistolUnlocked, 0);
    public static WeaponStats SubMachineGun = new WeaponStats(1, 0.4f, 0.30f, 200, 400, "SMG", WeaponType.GUN, SpaceBlasterGame.Settings.SubMachineGunUnlocked, 300);
    public static WeaponStats Shotgun = new WeaponStats(3, 0.5f, 0.4f, 500, 500, "Shotgun", WeaponType.GUN, SpaceBlasterGame.Settings.ShotgunUnlocked, 0);
    public static WeaponStats MachineGun = new WeaponStats(1, 0.3f, 0.3f, 250, 600, "Machine gun", WeaponType.GUN, SpaceBlasterGame.Settings.MachineGunUnlocked, 300);
    //public static WeaponStats FlameThrower = new WeaponStats(1, 1, 0.3f, 250, 600, "FlameThrower", WeaponType.FLAMETHROWER, SpaceBlasterGame.Settings.MachineGunUnlocked, 300);
    public static WeaponStats FlameThrower = new WeaponStats(3, 0.5f, 0.5f, 100, 500, "Flamethrower", WeaponType.FLAMETHROWER, SpaceBlasterGame.Settings.FlameThrowerUnlocked, 1000);

    // enemy weapons
    public static WeaponStats EnemyShotgun = new WeaponStats(3, 0.25f, 0.3f, 600, 200, "");
    public static WeaponStats EnemyMachineGun = new WeaponStats(1, 0.25f, 0.30f, 300, 200, "");
    public static WeaponStats EnemyPistol = new WeaponStats(1, 0.5f, 0.25f, 700, 300, "");
}

public class Weapon : Position, IArtificialIntelligence, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Vector2 Direction = new Vector2(0, 0);
    private double CurrentTimer = 0;
    private double CurrentSFXTimer = 0;
    public bool CanShoot = true;
    private Random rand = new Random();
    public WeaponStats stats;

    ParticleEmitter flames;
    DamageArea damageArea;
    bool shooting = false;

    public Weapon(WeaponStats wstats, float x, float y, Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(2, 35), 1, 0.2, 5, 4, 3, 0f, new Vector2(0, 0)) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        flames = new ParticleEmitter(
            -8,
            -8,
            new Sprite(SpaceBlasterGame.TextureAtlas, new Rectangle(21, 132, 6, 6), new Vector2(0, 0), 3),
            new Vector2(1, 0),
            wstats.Spread,
            wstats.ProjectileSpeed / 2,
            wstats.ProjectileSpeed,
            wstats.ProjectileCount,
            false,
            (float)wstats.FireRate,
            400,
            1,
            0,
            1f,
            3f,
            Color.Red,
            Color.Orange,
            this.scene,
            this
        );

        flames.emit = false;
        this.scene.Spawn(flames);

        damageArea = new DamageArea(0, 0, 48 * 2, 48 * 2, this.scene, this);
        damageArea.Damage = wstats.Damage;
        damageArea.active = false;

        this.scene.Spawn(damageArea);

        stats = wstats;
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        if (this.stats.type == WeaponType.GUN)
        {
            if (CanShoot == false) CurrentTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (CurrentTimer > stats.FireRate) CanShoot = true;
        }
        else if (this.stats.type == WeaponType.FLAMETHROWER)
        {
            CurrentSFXTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (shooting && CurrentSFXTimer > stats.FireRate)
            {
                SoundEffectsManager.Play(this,"flamethrower");
                CurrentSFXTimer = 0;
            }

            flames.dir = this.Direction;

            if (shooting) damageArea.Pos = (new Vector2(-48, -48)) + flames.dir * 48 * 1.5f;

            flames.emit = shooting;
            damageArea.active = shooting;
        }
    }

    public void UpdateStats()
    {
        damageArea.Damage = this.stats.Damage;
        flames.spread = this.stats.Spread;
        flames.maxSpeed = this.stats.ProjectileSpeed;
        flames.minSpeed = this.stats.ProjectileSpeed / 2;
        flames.spawnInterval = this.stats.FireRate;
        flames.howMany = this.stats.ProjectileCount;
        flames.emit = false;
        shooting = false;
    }

    public void Aim(Vector2 dir)
    {
        Direction = dir;

        var spr = this.AnimationPlayer.GetCurrentSprite();
        spr.Rotation = (float)Math.Atan2(this.Direction.Y, this.Direction.X);

        if (this.Direction.X < 0) spr.FlipH();
        else spr.Unflip();
    }

    public virtual void Shoot()
    {
        if (this.stats.type == WeaponType.GUN) {
            if (CanShoot)
            {
                if (this.Parent != null)
                {
                    for (int i = 0; i < stats.ProjectileCount; i++)
                    {
                        Vector2 dir = Direction;
                        dir = Vector2.Rotate(dir, stats.Spread * (float)rand.NextDouble() * (rand.Next(2) == 1 ? -1 : 1));

                        if (this.Parent is Player) this.scene.Spawn(new PlayerProjectile(this.stats.Damage, this.Pos.X, this.Pos.Y, dir, this.stats.ProjectileSpeed, null));
                        else if (this.Parent is Enemy) this.scene.Spawn(new Projectile(this.stats.Damage, this.Pos.X, this.Pos.Y, dir, this.stats.ProjectileSpeed, null));
                    }
                }

                CanShoot = false;
                CurrentTimer = 0;
                SoundEffectsManager.Play(this, "player_shoots");
            }
        }
        else if (this.stats.type == WeaponType.FLAMETHROWER)
        {
            shooting = true;
        }
    }
    public void StopShoot()
    {
        if (this.stats.type == WeaponType.FLAMETHROWER) shooting = false;
    }
}
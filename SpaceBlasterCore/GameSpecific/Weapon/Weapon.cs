using Microsoft.Xna.Framework;
namespace GameSpecific;

using System;
using System.Collections.Generic;
using Scene.Components;
using SpaceBlaster;
using SpaceBlaster.Scene;

public struct WeaponStats
{
    public WeaponStats(int projectileCount, float damage, float spread, double firerate, int speed, string displayName, bool unlocked = true, int price = 0)
    {
        this.ProjectileCount = projectileCount;
        this.Damage = damage;
        this.Spread = spread;
        this.FireRate = firerate;
        this.ProjectileSpeed = speed;
        this.Price = price;
        this.DisplayName = displayName;
        this.Unlocked = unlocked;
    }
    public int ProjectileCount;
    public float Damage;
    public float Spread;
    public double FireRate;
    public int ProjectileSpeed;
    public int Price = 0;
    public bool Unlocked;
    public string DisplayName = "";
}

public struct PremadeWeapons
{
    //player weapons
    public static WeaponStats Pistol = new WeaponStats(1, 1, 0.1f, 600, 600, "Pistol", SpaceBlasterGame.Settings.PistolUnlocked, 0);
    public static WeaponStats SubMachineGun = new WeaponStats(1, 0.3f, 0.30f, 200, 400, "SMG", SpaceBlasterGame.Settings.SubMachineGunUnlocked, 300);
    public static WeaponStats Shotgun = new WeaponStats(3, 0.5f, 0.4f, 500, 500, "Shotgun", SpaceBlasterGame.Settings.ShotgunUnlocked, 0);
    public static WeaponStats MachineGun = new WeaponStats(1, 1, 0.3f, 250, 600, "Machine gun", SpaceBlasterGame.Settings.MachineGunUnlocked, 300);

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
    public bool CanShoot = true;
    private Random rand = new Random();
    public WeaponStats stats;

    public Weapon(WeaponStats wstats, float x, float y, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(2, 35), 1, 0.2, 5, 4, 3, 0f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        stats = wstats;
    }

    public void HandleAI(GameTime gameTime, Scene scene)
    {
        if (CanShoot == false) CurrentTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (CurrentTimer > stats.FireRate) CanShoot = true;
    }

    public void Aim(Vector2 dir)
    {
        Direction = dir;

        var spr = this.AnimationPlayer.GetCurrentSprite();
        spr.Rotation = (float)Math.Atan2(this.Direction.Y, this.Direction.X);

        if (this.Direction.X < 0) spr.FlipH();
        else spr.Unflip();
    }

    public void Shoot()
    {
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
}
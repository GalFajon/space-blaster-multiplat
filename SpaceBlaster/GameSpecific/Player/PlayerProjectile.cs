using Microsoft.Xna.Framework;
namespace GameSpecific;

using System.Collections.Generic;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using SpaceBlaster;

public class PlayerProjectile : Physics, IAnimatable
{

    public float Damage = 1;
    public AnimationPlayer AnimationPlayer { get; set; }
    public PlayerProjectile(float damage, float x, float y, Vector2 dir, float speed, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 18, 18), true, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(22, 133), 1, 0.2, 6, 6, 3, 0f, new Vector2(0, 0)) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Direction = dir;
        this.Speed = speed;
        this.Damage = damage;
        this.IsSolid = false;
    }

    public override void HandleCollision(Collider collider)
    {
        if (collider is not Player && collider is not PlayerProjectile && collider is not Projectile && collider is not CoverWall && collider is not Currency && collider is not DamageArea)
        {
            if (collider is Enemy) SoundEffectsManager.Play(this, "enemy_hit");
            this.Destroy();
        }
        else return;
    }
}
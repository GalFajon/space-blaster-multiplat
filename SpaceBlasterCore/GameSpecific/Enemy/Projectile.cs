using Microsoft.Xna.Framework;
namespace GameSpecific;

using System.Collections.Generic;
using Scene.Components;
using SpaceBlaster;

public class Projectile : Physics, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public float Damage = 1;
    public Projectile(float damage, float x, float y, Vector2 dir, float speed, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 6, 6), true, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(21, 139), 1, 0.2, 6, 6, 3, 0f) },
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
        if ((collider is Player) || (collider is Wall)) this.Destroy();
    }
}
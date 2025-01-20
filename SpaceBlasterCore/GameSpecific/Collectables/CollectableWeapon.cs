using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster;
namespace GameSpecific;

public class CollectableWeapon : Collider, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    WeaponStats Stats;

    public CollectableWeapon(WeaponStats stats, float x, float y, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 32, 32), scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(53, 20), 1, 0.2, 10, 10, 3, 0f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        Stats = stats;
        this.Pos = new Vector2(x, y);
        this.IsSolid = false;
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is Player p)
        {
            SoundEffectsManager.Play(this, "health_collected");
            p.currentWeapon.stats = this.Stats;
            this.Destroy();
        }
        else return;
    }
}
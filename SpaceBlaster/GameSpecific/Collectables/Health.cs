using System.Collections.Generic;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;

public class Health : Collider, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Health(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 32, 32), scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(37, 20), 1, 0.2, 16, 10, 3, 0f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Pos = new Vector2(x, y);
        this.IsSolid = false;
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is Player p)
        {
            SoundEffectsManager.Play(this, "health_collected");
            p.Health += 1;
            this.Destroy();
        }
    }
}
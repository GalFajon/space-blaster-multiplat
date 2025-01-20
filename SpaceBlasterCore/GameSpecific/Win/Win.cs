using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster;
namespace GameSpecific;

public class Win : Collider, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Win(float x, float y, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 32, 32), scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(35, 3), 3, 0.2, 16, 16, 3, 0f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Pos = new Vector2(x, y);
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is Player)
        {
            SoundEffectsManager.Play(this, "win_collected");

            if (this.scene is GameSpecific.Level l) l.Win();
            this.Destroy();
        }
    }
}
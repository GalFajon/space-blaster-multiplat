using Scene.Components;
using SpaceBlaster;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class Explosion : Position, IAnimatable, IArtificialIntelligence
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Explosion(float x, float y, SpaceBlaster.Scene.Scene scene) : base(x, y, scene)
    {
        var expl = new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(0, 90), 9, 0.1, 24, 24, 3, 0f);
        expl.loop = false;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, expl },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
        SoundEffectsManager.Play(this, "explosion");
    }

    public virtual void HandleAI(GameTime gameTime, SpaceBlaster.Scene.Scene level)
    {
        if (this.AnimationPlayer.GetCurrentSprite().finished) this.Destroy();
    }
}
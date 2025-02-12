using SpaceBlaster;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Gameplay.Audio;

public class Explosion : Position, IAnimatable, IArtificialIntelligence
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Explosion(float x, float y, Scene scene) : base(x, y, scene)
    {
        var expl = new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(0, 90), 9, 0.1, 24, 24, 3, 0f, new Vector2(0, 0));
        expl.loop = false;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, expl },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
        SoundEffectsManager.Play(this, "explosion");
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        if (this.AnimationPlayer.GetCurrentSprite().finished) this.Destroy();
    }
}
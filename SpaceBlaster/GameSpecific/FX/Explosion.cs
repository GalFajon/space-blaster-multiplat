using SpaceBlaster;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Gameplay.Audio;
using GameSpecific;

public class Explosion : Position, IAnimatable, IArtificialIntelligence
{
    private DamageArea damageArea = null;
    public AnimationPlayer AnimationPlayer { get; set; }
    public Explosion(float x, float y, Scene scene, bool damaging = false, int scale = 3) : base(x, y, scene)
    {
        var expl = new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(0, 90), 9, 0.1, 24, 24, scale, 0f, new Vector2(0, 0));
        expl.loop = false;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, expl },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
        SoundEffectsManager.Play(this, "explosion");

        if (damaging == true)
        {
            damageArea = new DamageArea(0, 0, 24 * scale, 24 * scale, this.scene, this);
            damageArea.active = true;
            damageArea.Damage = 3;
            this.scene.Spawn(damageArea);
        }
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        if (this.AnimationPlayer.GetCurrentSprite().finished)
        {
            if (this.damageArea != null) this.damageArea.Destroy();
            this.Destroy();
        }
    }
}
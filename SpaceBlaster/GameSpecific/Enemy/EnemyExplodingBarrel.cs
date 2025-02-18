using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using GameEngine.Graphics;
using GameEngine.Gameplay;
using GameEngine.Gameplay.Audio;

namespace GameSpecific;

public class EnemyExplodingBarrel : Enemy, IAnimatable
{
    public EnemyExplodingBarrel(float x, float y, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(3, x, y, new Rectangle(0, 0, 48, 48), true, scene, parent)
    {
        this.MaxHealth = 1;
        this.Health = 1;

        this.currencyValue = 5;
        this.IsSolid = false;
        this.StunTimer = 300;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(53, 32), 1, 0.2, 11, 14, 3, 0f, new Vector2(0, 0)) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
        this.damagingExplosion = true;
    }

    public override void HandleAI(GameTime gameTime)
    {
        base.HandleAI(gameTime);
        if (this.IsHit) return;
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is not Projectile)
        {
            base.HandleCollision(collided);
        }
    }
}
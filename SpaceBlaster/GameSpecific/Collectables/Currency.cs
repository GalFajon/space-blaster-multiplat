using System.Collections.Generic;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;

public class Currency : Collider, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    private int value;
    public Currency(int value, float x, float y, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 48, 48), scene, parent)
    {
        this.value = value;
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(53, 20), 1, 0.2, 10, 10, 3, 0.05f, new Vector2(0, 0)) },
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
            SpaceBlasterGame.Settings.Currency += value;
            this.Destroy();
        }
    }
}
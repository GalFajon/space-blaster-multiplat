using System.Collections.Generic;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;

public class Door : Collider, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Door(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 40, 40), scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 1, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(140, 34), 1, 0.2, 16, 16, 3, 0.1f, new Vector2(0, 0)) },
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(0, 134), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) }
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(1);

        this.Pos = new Vector2(x, y);
        this.Enabled = true;
    }

    public void Open()
    {
        this.Enabled = false;
        this.AnimationPlayer.SetCurrentAnimation(1);
        SoundEffectsManager.Play(this, "door_open");
    }

    public void Close()
    {
        this.Enabled = true;
        this.AnimationPlayer.SetCurrentAnimation(0);
    }
}
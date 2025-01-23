using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;

public class Door : Collider, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Door(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 32, 32), scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(52, 133), 1, 0.2, 16, 16, 3, 0f) },
                { 1, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(70, 133), 1, 0.2, 16, 16, 3, 0f) }
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
    }

    public void Close()
    {
        this.Enabled = true;
        this.AnimationPlayer.SetCurrentAnimation(0);
    }
}
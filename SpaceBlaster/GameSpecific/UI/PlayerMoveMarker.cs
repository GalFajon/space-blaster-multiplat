using Microsoft.Xna.Framework;
namespace GameSpecific;

using System;
using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using SpaceBlaster;

public class PlayerMoveMarker : Position, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Vector2 Dir { get; set; }

    public PlayerMoveMarker(float x, float y, Scene scene, SceneObject parent) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(67, 36), 1, 0.2, 16, 32, 3, 0f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

    }
    public void Aim(Vector2 dir)
    {
        Dir = dir;

        var spr = this.AnimationPlayer.GetCurrentSprite();
        spr.Rotation = (float)Math.Atan2(this.Dir.Y, this.Dir.X);
    }
}
using Microsoft.Xna.Framework;
namespace GameSpecific;

using System;
using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using SpaceBlaster;

public class JoystickMarker : Position, IUIAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Vector2 Dir { get; set; }

    public JoystickMarker(float x, float y, Scene scene, SceneObject parent) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(38, 57), 1, 0.2, 16, 16, 3, 0f, new Vector2(8,8)) },
                { 1, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(38, 75), 1, 0.2, 16, 16, 3, 0f, new Vector2(8,8)) },
                { 2, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(54, 57), 1, 0.2, 16, 16, 3, 0f, new Vector2(8,8)) },
                { 3, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(54, 75), 1, 0.2, 16, 16, 3, 0f, new Vector2(8,8)) }
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
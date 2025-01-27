using SpaceBlaster;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using GameEngine.Graphics;
using GameEngine.Scene;

public class LevelBackground : Position, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public LevelBackground(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.Background, new Vector2(0, 0), 1, 0.1, 480, 270, 5, 1f, new Vector2(0, 0)) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
    }
}
using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;

namespace GameSpecific;

public class BackgroundWall : Position, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public BackgroundWall(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(140, 34), 1, 0.2, 16, 16, 3, 0.2f, new Vector2(0, 0)) }
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Pos = new Vector2(x, y);
    }
}
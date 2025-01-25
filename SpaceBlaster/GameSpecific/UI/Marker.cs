using Microsoft.Xna.Framework;
namespace GameSpecific;
using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using SpaceBlaster;

public class Marker : Position, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Marker(float x, float y, Scene scene, SceneObject parent) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(67, 19), 1, 0.2, 14, 14, 3, 0f, new Vector2(0, 0)) },
            }
        );

    }
}
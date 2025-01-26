using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;

public class Wall : Collider, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Wall(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 40, 40), scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(0, 134), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Pos = new Vector2(x, y);
    }
}
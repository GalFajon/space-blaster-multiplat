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
    public Wall(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 48, 48), scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(0, 134), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 1, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(154, 0), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 2, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(154, 16), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 3, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(170, 0), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 4, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(186, 0), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 5, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(170, 16), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 6, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(186, 16), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },

                { 7, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(157, 34), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 8, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(173, 34), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 9, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(157 + (16 * 2), 34), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
                { 10, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(157 + (16 * 3), 34), 1, 0.2, 16, 16, 3, 0f, new Vector2(0, 0)) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Pos = new Vector2(x, y);
    }
}
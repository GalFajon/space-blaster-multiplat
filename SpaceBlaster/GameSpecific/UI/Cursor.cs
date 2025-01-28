using Microsoft.Xna.Framework;
namespace GameSpecific;
using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using SpaceBlaster;
using GameEngine.Gameplay.Input;

public class Cursor : Position, IUIAnimatable, IInputHandler
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Cursor(float x, float y, Scene scene, SceneObject parent) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(88, 69), 1, 0.2, 16, 16, 2, 0f, new Vector2(0, 0)) },
            }
        );

        this.Pos = InputManager.MousePosUI;
    }

    public void HandleInput(GameTime gameTime, TouchCollection touch, KeyboardState keyboard, MouseState mouse, Matrix UITransform, Matrix RendererTransform) {
        this.Pos = InputManager.MousePosUI;
    }
}
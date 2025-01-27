using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
namespace GameEngine.Scene.Components;

public interface IInputHandler
{
    public void HandleInput(GameTime gameTime, TouchCollection touch, KeyboardState keyboard, MouseState mouse, Matrix UITransform, Matrix RendererTransform) { return; }
}
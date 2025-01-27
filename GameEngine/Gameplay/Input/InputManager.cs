using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameEngine.Scene.Components;

namespace GameEngine.Gameplay.Input;

public class InputManager : GameComponent
{
    public static Vector2 MousePosWorld = Vector2.Zero;
    public static Vector2 MousePosUI = Vector2.Zero;

    private GameBase _game;
    private GraphicsDeviceManager _graphics;

    public InputManager(GameBase game, GraphicsDeviceManager graphics) : base(game)
    {
        _game = game;
        _graphics = graphics;
    }

    public override void Update(GameTime gameTime)
    {
        TouchCollection t = TouchPanel.GetState();
        KeyboardState state = Keyboard.GetState();
        MouseState mouse = Mouse.GetState();

        MousePosWorld = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(_game.Renderer.GetRendererTransformationMatrix()));
        MousePosUI = Vector2.Transform(new Vector2(mouse.X, mouse.Y), Matrix.Invert(_game.RendererUI.GetUIRendererTransformationMatrix()));

        foreach (SceneObject item in _game.CurrentScene.Components) if (item is IInputHandler i) i.HandleInput(gameTime, t, state, mouse, _game.RendererUI.GetUIRendererTransformationMatrix(), _game.Renderer.GetRendererTransformationMatrix());

        base.Update(gameTime);
    }
}
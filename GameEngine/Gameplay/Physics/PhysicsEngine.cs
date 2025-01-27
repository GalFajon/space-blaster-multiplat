using Microsoft.Xna.Framework;
using GameEngine.Gameplay.Input;

namespace GameEngine.Gameplay.Physics;

public class PhysicsEngine : GameComponent
{

    private GameBase _game;

    public PhysicsEngine(GameBase game, InputManager input) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (object item in _game.CurrentScene.Components)
        {
            if (item is GameEngine.Scene.Components.Physics phy)
            {
                phy.Velocity = phy.Direction * phy.Speed;
                phy.Pos = new Vector2(
                    phy.LocalPos.X + phy.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds,
                    phy.LocalPos.Y + phy.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds
                );
            }
        }

        base.Update(gameTime);
    }
}
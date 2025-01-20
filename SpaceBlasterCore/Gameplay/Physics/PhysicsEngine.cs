using Microsoft.Xna.Framework;
using SpaceBlaster.Scene;
using GameSpecific;
using Scene.Components;
using SpaceBlaster;

public class PhysicsEngine : GameComponent
{

    private SpaceBlasterGame _game;

    public PhysicsEngine(SpaceBlasterGame game, InputManager input) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (object item in _game._currentLevel.Components)
        {
            if (item is Physics phy)
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
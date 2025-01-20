using System;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster.Scene;
using SpaceBlaster;

public class CollisionHandler : GameComponent
{

    private SpaceBlasterGame _game;

    public CollisionHandler(SpaceBlasterGame game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (SceneObject item1 in _game._currentLevel.Components)
        {
            if (item1 is Collider c1)
            {
                c1.Colliding = false;

                foreach (SceneObject item2 in _game._currentLevel.Components)
                {
                    if (item2 != item1)
                    {
                        if (item2 is Collider c2)
                        {
                            if (Collider.DetectCollision(c1, c2))
                            {
                                c1.Colliding = true;
                                c1.HandleCollision(c2);

                                if (c1 is Physics phy)
                                {
                                    if (phy.IsSolid && c2.IsSolid)
                                    {
                                        phy.Pos = new Vector2(
                                            phy.LocalPos.X - phy.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds,
                                            phy.LocalPos.Y - phy.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds
                                        );
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        base.Update(gameTime);
    }
}
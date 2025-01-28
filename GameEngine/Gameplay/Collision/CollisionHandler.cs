using Microsoft.Xna.Framework;
using GameEngine.Scene.Components;
using System.Diagnostics;
using System;

namespace GameEngine.Gameplay.Collision;

public class CollisionHandler : GameComponent
{

    private GameBase _game;

    public CollisionHandler(GameBase game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (SceneObject item1 in _game.CurrentScene.Components)
        {
            if (item1 is Collider c1)
            {
                c1.Colliding = false;

                foreach (SceneObject item2 in _game.CurrentScene.Components)
                {
                    if (item2 != item1)
                    {
                        if (item2 is Collider c2)
                        {
                            if (c1 is GameEngine.Scene.Components.Physics phy)
                            {
                                var oldx = phy.LocalPos.X;
                                var oldy = phy.LocalPos.Y;

                                var x = phy.LocalPos.X + phy.Direction.X * phy.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                var y = phy.LocalPos.Y + phy.Direction.Y * phy.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                                if (phy.IsSolid && c2.IsSolid)
                                {
                                    bool colliding = false;

                                    var dirx = phy.Direction.X;
                                    var diry = phy.Direction.Y;

                                    c1.Pos = new Vector2(oldx, y);
                                    if (Collider.DetectCollision(c1, c2)) { diry = 0; colliding = true; }
                                    
                                    c1.Pos = new Vector2(x, oldy);
                                    if (Collider.DetectCollision(c1, c2)) { dirx = 0; colliding = true; }
                                    
                                    phy.Direction = new Vector2(dirx, diry);
                                    phy.Pos = new Vector2(oldx, oldy);

                                    if (colliding)
                                    {
                                        c1.Colliding = true;
                                        c1.HandleCollision(c2);
                                    }
                                }
                                else
                                {
                                    if (Collider.DetectCollision(c1, c2))
                                    {
                                        c1.Colliding = true;
                                        c1.HandleCollision(c2);
                                    }
                                }
                            }
                            else
                            {
                                if (Collider.DetectCollision(c1, c2))
                                {
                                    c1.Colliding = true;
                                    c1.HandleCollision(c2);
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
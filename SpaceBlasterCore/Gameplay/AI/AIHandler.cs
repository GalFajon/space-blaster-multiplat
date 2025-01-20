using System;
using System.Diagnostics;
using GameSpecific;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster.Scene;
using SpaceBlaster;

public class AIHandler : GameComponent
{

    private SpaceBlasterGame _game;

    public AIHandler(SpaceBlasterGame game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (SceneObject item in _game._currentLevel.Components)
        {
            if (item is IArtificialIntelligence a)
            {
                if (item is Position p)
                {
                    //if (_game.isOnScreen(p.Pos))
                    a.HandleAI(gameTime, _game._currentLevel);
                }
                else a.HandleAI(gameTime, _game._currentLevel);
            }
        }

        base.Update(gameTime);
    }
}
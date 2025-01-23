using System;
using System.Diagnostics;
using GameSpecific;
using Microsoft.Xna.Framework;
using GameEngine.Scene;
using GameEngine;
using GameEngine.Scene.Components;

namespace GameEngine.Gameplay.AI;

public class AIHandler : GameComponent
{

    private GameBase _game;

    public AIHandler(GameBase game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (SceneObject item in _game.CurrentScene.Components)
        {
            if (item is IArtificialIntelligence a)
            {
                if (item is Position p)
                {
                    //if (_game.isOnScreen(p.Pos))
                    a.HandleAI(gameTime);
                }
                else a.HandleAI(gameTime);
            }
        }

        base.Update(gameTime);
    }
}
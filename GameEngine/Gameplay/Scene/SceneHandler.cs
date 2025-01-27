using Microsoft.Xna.Framework;
using System.Collections;
using GameEngine.Scene.Components;

namespace GameEngine.Gameplay.Scene;

public class SceneHandler : GameComponent
{

    private GameBase _game;

    public SceneHandler(GameBase game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        ArrayList toRemove = new ArrayList { };

        foreach (SceneObject item in _game.CurrentScene.Components)
        {
            if (item.ToDestroy) toRemove.Add(item);
            if (item.Parent != null && item.Parent.ToDestroy) toRemove.Add(item);
        }

        foreach (SceneObject item in toRemove) _game.CurrentScene.Components.Remove(item);
        foreach (SceneObject item in _game.CurrentScene.ToSpawn) _game.CurrentScene.Components.Add(item);

        _game.CurrentScene.ToSpawn = new ArrayList() { };

        base.Update(gameTime);
    }
}
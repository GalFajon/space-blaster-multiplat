using Microsoft.Xna.Framework;
using SpaceBlaster.Scene;
using GameSpecific;
using Scene.Components;
using System.Collections;
using SpaceBlaster;

public class SceneHandler : GameComponent
{

    private SpaceBlasterGame _game;

    public SceneHandler(SpaceBlasterGame game) : base(game)
    {
        _game = game;
    }

    public override void Update(GameTime gameTime)
    {
        //GAMEPLAY
        ArrayList toRemove = new ArrayList { };

        foreach (SceneObject item in _game._currentLevel.Components)
        {
            if (item.ToDestroy) toRemove.Add(item);
            if (item.Parent != null && item.Parent.ToDestroy) toRemove.Add(item);
        }

        foreach (SceneObject item in toRemove) _game._currentLevel.Components.Remove(item);
        foreach (SceneObject item in _game._currentLevel.ToSpawn) _game._currentLevel.Components.Add(item);

        _game._currentLevel.ToSpawn = new ArrayList() { };

        base.Update(gameTime);
    }
}
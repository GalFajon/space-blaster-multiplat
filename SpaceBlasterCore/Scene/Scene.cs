using System.Collections;
using Scene.Components;
using Microsoft.Xna.Framework;
using GameSpecific;
using System.Collections.Generic;

namespace SpaceBlaster.Scene;

public class Scene : GameComponent
{
    public Camera Camera = null;
    public List<SceneObject> _components = new List<SceneObject> { };
    public ArrayList _spawnList = new ArrayList { };

    public Scene(SpaceBlasterGame game, Camera camera = null) : base(game)
    {
        if (camera != null) this.Camera = camera;
    }

    public List<SceneObject> Components
    {
        get => _components;
        set
        {
            _components = value;
        }
    }

    public ArrayList ToSpawn
    {
        get => _spawnList;
        set => _spawnList = value;
    }

    public void Spawn(SceneObject obj)
    {
        obj.scene = this;
        _spawnList.Add(obj);
    }
}
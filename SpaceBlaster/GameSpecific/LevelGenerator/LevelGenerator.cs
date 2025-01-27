using System;
using System.Collections.Generic;
using GameEngine.Gameplay;
using GameEngine.Scene.Components;
using GameSpecific.LevelGenerator.Rooms;
using Microsoft.Xna.Framework;
namespace GameSpecific.LevelGenerator;

public enum LevelGeneratorDir
{
    UP,
    DOWN,
    RIGHT
}

public class LevelGenerator
{

    static LevelGeneratorDir _currentDir = LevelGeneratorDir.UP;
    static LevelGeneratorDir _prevDir = LevelGeneratorDir.RIGHT;

    public static Room getRandomRoom(Level scene)
    {
        int i = RandomNumber.GetRandomNumber(0, RoomLayouts.maxRoom);
        return RoomLayouts.GenerateRoom(i, scene);
    }

    public static List<SceneObject> Generate(int rooms, Level scene)
    {
        List<Room> _rooms = new List<Room>();
        List<SceneObject> _components = new List<SceneObject>();
        Room prev = null;

        for (int i = 0; i <= rooms; i++)
        {
            Room r;

            if (i == 0) r = new Room(0, 0, 12, 12, new Dictionary<Vector2, Position>() { }, RoomGoals.NONE, scene);
            else if (i == rooms) r = new Room(0, 0, 12, 12, new Dictionary<Vector2, Position>() { { new Vector2(6, 6), new Win(0, 0, scene, null) } }, RoomGoals.NONE, scene);
            else r = getRandomRoom(scene);

            Array values = Enum.GetValues(typeof(LevelGeneratorDir));

            if (i > 0) _currentDir = (LevelGeneratorDir)values.GetValue(RandomNumber.GetRandomNumber(0, values.Length));
            if (_prevDir == LevelGeneratorDir.UP || _prevDir == LevelGeneratorDir.DOWN) _currentDir = LevelGeneratorDir.RIGHT;

            if (i != 0)
            {
                if (_prevDir == LevelGeneratorDir.UP) r.BottomDoor = true;
                else if (_prevDir == LevelGeneratorDir.DOWN) r.TopDoor = true;
                else if (_prevDir == LevelGeneratorDir.RIGHT) r.LeftDoor = true;
            }

            if (i != rooms)
            {
                if (_currentDir == LevelGeneratorDir.UP) r.TopDoor = true;
                else if (_currentDir == LevelGeneratorDir.DOWN) r.BottomDoor = true;
                else if (_currentDir == LevelGeneratorDir.RIGHT) r.RightDoor = true;
            }

            r.generateBounds(_prevDir);

            if (prev != null)
            {
                float x = prev.Pos.X;
                float y = prev.Pos.Y;

                if (_prevDir == LevelGeneratorDir.UP) y -= 48 * 13;
                else if (_prevDir == LevelGeneratorDir.DOWN) y += 48 * 13;
                else if (_prevDir == LevelGeneratorDir.RIGHT) x += 48 * 13;

                r.Pos = new Vector2(x, y);
            }
            else r.Pos = new Vector2(0, 0);

            _prevDir = _currentDir;
            prev = r;

            _rooms.Add(r);
            _components.AddRange(r.Components);
        }

        return _components;
    }
}
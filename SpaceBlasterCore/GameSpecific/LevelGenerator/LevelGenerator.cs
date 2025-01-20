using System;
using System.Collections;
using System.Collections.Generic;
using GameSpecific.LevelGenerator.Rooms;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster.Scene;
namespace GameSpecific.LevelGenerator;

public enum LevelGeneratorDir
{
    UP,
    DOWN,
    //LEFT,
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

        /*if (i == 0) return new Room(0, 0, 12, 12, new Dictionary<Vector2, Position>
        {
            {new Vector2(3, 3),new EnemyBee(0, 0, scene, null)},
            {new Vector2(9, 9),new EnemyBee(0, 0, scene, null)}
        }, RoomGoals.KILL_ENEMIES, scene);
        else if (i == 1) return new Room(0, 0, 12, 12, new Dictionary<Vector2, Position>
        {
            {new Vector2(6, 6),new EnemyZombie(0,0, scene, null)},
        }, RoomGoals.KILL_ENEMIES, scene);
        else if (i == 2) return new Room(0, 0, 12, 12, new Dictionary<Vector2, Position>() {
            { new Vector2(4, 1), new EnemySoldierD(PremadeWeapons.Pistol, 0, 0, scene, null) },
            { new Vector2(9, 9), new EnemySoldierD(PremadeWeapons.Pistol, 0, 0, scene, null) },
            { new Vector2(4, 2), new CoverWall(0, 0, scene, null) },
            { new Vector2(4, 3), new CoverWall(0, 0, scene, null) },
            { new Vector2(3, 3), new Wall(0, 0, scene, null) },
            { new Vector2(7, 3), new Wall(0, 0, scene, null) },
            { new Vector2(3, 7), new Wall(0, 0, scene, null) },
            { new Vector2(7, 7), new Wall(0, 0, scene, null) },
            { new Vector2(8, 7), new CoverWall(0, 0, scene, null) },
            { new Vector2(9, 7), new CoverWall(0, 0, scene, null) },
        }, RoomGoals.KILL_ENEMIES, scene);
        else return new Room(0, 0, 12, 12, new Dictionary<Vector2, Position>
        {
            {new Vector2(6, 6),new Health(0, 0, scene, null)},
        }, RoomGoals.NONE, scene);*/
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

            //if (_prevDir == LevelGeneratorDir.LEFT && _currentDir == LevelGeneratorDir.RIGHT) _currentDir = LevelGeneratorDir.LEFT;
            //else if (_prevDir == LevelGeneratorDir.RIGHT && _currentDir == LevelGeneratorDir.LEFT) _currentDir = LevelGeneratorDir.RIGHT;
            if (_prevDir == LevelGeneratorDir.UP || _prevDir == LevelGeneratorDir.DOWN) _currentDir = LevelGeneratorDir.RIGHT;

            if (i != 0)
            {
                if (_prevDir == LevelGeneratorDir.UP) r.BottomDoor = true;
                else if (_prevDir == LevelGeneratorDir.DOWN) r.TopDoor = true;
                //else if (_prevDir == LevelGeneratorDir.LEFT) r.RightDoor = true;
                else if (_prevDir == LevelGeneratorDir.RIGHT) r.LeftDoor = true;
            }

            if (i != rooms)
            {
                if (_currentDir == LevelGeneratorDir.UP) r.TopDoor = true;
                else if (_currentDir == LevelGeneratorDir.DOWN) r.BottomDoor = true;
                //else if (_currentDir == LevelGeneratorDir.LEFT) r.LeftDoor = true;
                else if (_currentDir == LevelGeneratorDir.RIGHT) r.RightDoor = true;
            }

            r.generateBounds(_prevDir);

            if (prev != null)
            {
                float x = prev.Pos.X;
                float y = prev.Pos.Y;

                if (_prevDir == LevelGeneratorDir.UP) y -= 48 * 13;
                else if (_prevDir == LevelGeneratorDir.DOWN) y += 48 * 13;
                //else if (_prevDir == LevelGeneratorDir.LEFT) x -= 48 * 12;
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
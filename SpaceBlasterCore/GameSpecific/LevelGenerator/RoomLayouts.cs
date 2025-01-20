using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using GameSpecific.LevelGenerator.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Scene.Components;
using SpaceBlaster;
using SpaceBlaster.Scene;
namespace GameSpecific.LevelGenerator;

public class JSONRoomLayoutObject
{
    public string Type { get; set; }
    public List<int> Coordinates { get; set; }
}

public class JSONRoomLayout
{
    public string Goal { get; set; }
    public List<JSONRoomLayoutObject> Objects { get; set; }
}

public static class RoomLayouts
{
    public static int maxRoom = 10;
    public static ContentManager Content = null;
    public static Room GenerateRoom(int i, Level scene)
    {
#if WINDOWS
        if (File.Exists("./Content/room-" + i + ".json"))
        {
            var jsonRoom = JsonSerializer.Deserialize<JSONRoomLayout>(File.ReadAllText("./Content/room-" + i + ".json"));
            Dictionary<Vector2, Position> objects = new Dictionary<Vector2, Position>();

            foreach (JSONRoomLayoutObject l in jsonRoom.Objects)
            {
                if (l.Type == "wall") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new Wall(0, 0, scene, null);
                else if (l.Type == "coverwall") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new CoverWall(0, 0, scene, null);
                else if (l.Type == "bee") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemyBee(0, 0, scene, null);
                else if (l.Type == "soldier_shotgun") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemySoldierD(PremadeWeapons.EnemyShotgun, 0, 0, scene, null);
                else if (l.Type == "soldier_pistol") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemySoldierD(PremadeWeapons.EnemyPistol, 0, 0, scene, null);
                else if (l.Type == "soldier_machinegun") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemySoldierD(PremadeWeapons.EnemyMachineGun, 0, 0, scene, null);
                else if (l.Type == "zombie") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemyZombie(0, 0, scene, null);
                else if (l.Type == "health") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new Health(0, 0, scene, null);
                else if (l.Type == "currency") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new Currency(10, 0, 0, scene, null);
            }

            RoomGoals r = RoomGoals.NONE;

            if (jsonRoom.Goal == "kill_enemies") r = RoomGoals.KILL_ENEMIES;

            return new Room(0, 0, 12, 12, objects, r, scene);

        }
        else return new Room(0, 0, 12, 12, new Dictionary<Vector2, Position>(), RoomGoals.NONE, null);
#endif
#if ANDROID
        var jsonRoom = JsonSerializer.Deserialize<JSONRoomLayout>((new StreamReader(Game.Activity.Assets.Open("Content/room-" + i + ".json")).ReadToEnd()));
        Dictionary<Vector2, Position> objects = new Dictionary<Vector2, Position>();

        foreach (JSONRoomLayoutObject l in jsonRoom.Objects)
        {
            if (l.Type == "wall") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new Wall(0, 0, scene, null);
            else if (l.Type == "coverwall") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new CoverWall(0, 0, scene, null);
            else if (l.Type == "bee") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemyBee(0, 0, scene, null);
            else if (l.Type == "soldier_shotgun") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemySoldierD(PremadeWeapons.EnemyShotgun, 0, 0, scene, null);
            else if (l.Type == "soldier_pistol") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemySoldierD(PremadeWeapons.EnemyPistol, 0, 0, scene, null);
            else if (l.Type == "soldier_machinegun") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemySoldierD(PremadeWeapons.EnemyMachineGun, 0, 0, scene, null);
            else if (l.Type == "zombie") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new EnemyZombie(0, 0, scene, null);
            else if (l.Type == "health") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new Health(0, 0, scene, null);
            else if (l.Type == "currency") objects[new Vector2(l.Coordinates[0], l.Coordinates[1])] = new Currency(10, 0, 0, scene, null);
        }

        RoomGoals r = RoomGoals.NONE;

        if (jsonRoom.Goal == "kill_enemies") r = RoomGoals.KILL_ENEMIES;

        return new Room(0, 0, 12, 12, objects, r, scene);
#endif
    }
}
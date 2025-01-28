using System.Collections.Generic;
using System.Linq;
namespace GameSpecific.LevelGenerator.Rooms;

using System.Diagnostics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;

public enum RoomGoals
{
    NONE,
    KILL_ENEMIES
}

public class Room : Position
{
    private Dictionary<Vector2, Position> _grid = new Dictionary<Vector2, Position>() { };
    private List<BackgroundWall> Backgrounds = new List<BackgroundWall>();

    public List<Door> Doors = new List<Door>();

    public List<SceneObject> Components
    {
        get
        {
            var l = _grid.Values.ToList<SceneObject>();
            l.AddRange(Backgrounds.ToList<SceneObject>());
            return l;
        }
    }

    public bool LeftDoor, RightDoor, TopDoor, BottomDoor = false;
    readonly public int Rows = 0, Columns = 0;

    readonly public int tileWidth = 16 * 3, tileHeight = 16 * 3;
    public RoomGoals goal = RoomGoals.NONE;
    public bool goalComplete = false;

    public Room(float x, float y, int rows, int columns, Dictionary<Vector2, Position> grid, RoomGoals goal, Scene scene) : base(x, y, scene, null)
    {
        this.goal = goal;

        foreach (var key in grid.Keys)
        {
            if (grid[key] is Enemy d) d.Room = this;

            addToGrid((int)key.X, (int)key.Y, grid[key]);
        }

        if (goal == RoomGoals.NONE) goalComplete = true;

        this.Rows = rows;
        this.Columns = columns;
    }

    public Vector2 getTile(Vector2 pos)
    {
        return new Vector2((int)((pos.X + 16 - this.Pos.X) / this.tileWidth), (int)((pos.Y + 16 - this.Pos.Y) / this.tileHeight));
    }

    public Vector2 ToWorldPos(int x, int y)
    {
        return new Vector2(this.Pos.X + x * this.tileWidth, this.Pos.Y + y * tileHeight);
    }

    public void UpdateGoal()
    {
        if (this.goal == RoomGoals.KILL_ENEMIES)
        {
            int enemyCount = 0;

            for (int i = 0; i < Components.Count; i++) if (Components[i] is Enemy) enemyCount += 1;

            if (enemyCount <= 0)
            {
                foreach (var door in this.Doors) door.Open();
                goalComplete = true;
            }
        }
    }

    public Position getGrid(int x, int y)
    {
        Vector2 key = new Vector2(x, y);
        if (_grid.ContainsKey(key)) return _grid[new Vector2(x, y)];
        else return null;
    }

    public void removeFromGrid(SceneObject s)
    {
        foreach (KeyValuePair<Vector2, Position> s2 in _grid) if (s == s2.Value) _grid.Remove(s2.Key);
    }

    public void addToGrid(int x, int y, Position obj)
    {
        obj.Parent = this;
        obj.Pos = new Vector2(x * tileWidth + obj.Pos.X, y * tileHeight + obj.Pos.Y);
        _grid[new Vector2(x, y)] = obj;
    }

    public void addBackground(int x, int y, BackgroundWall obj)
    {
        obj.Parent = this;
        obj.Pos = new Vector2(x * tileWidth + obj.Pos.X, y * tileHeight + obj.Pos.Y);
        this.Backgrounds.Add(obj);
    }

    public void generateBounds(LevelGeneratorDir dir)
    {
        for (int i = 0; i <= Rows; i++)
        {
            for (int j = 0; j <= Columns; j++)
            {
                BackgroundWall bg = new BackgroundWall(0, 0, this.scene, this);
                bg.Parent = this;
                addBackground(i, j, bg);

                if (
                    i == 0 || i == Rows ||
                    j == 0 || j == Columns
                )
                {
                    if (
                        (i == Columns / 2 && j == 0 && TopDoor) ||
                        (j == Rows / 2 && i == 0 && LeftDoor) ||
                        (i == Columns / 2 && j == Rows && BottomDoor) ||
                        (j == Rows / 2 && i == Columns && RightDoor)
                    )
                    {
                        if (i == Columns / 2 && j == 0 && TopDoor && dir == LevelGeneratorDir.DOWN) continue;
                        if (i == Columns / 2 && j == Rows && BottomDoor && dir == LevelGeneratorDir.UP) continue;
                        if (j == Rows / 2 && i == 0 && LeftDoor && dir == LevelGeneratorDir.RIGHT) continue;

                        Door w = new Door(0, 0, this.scene, this);
                        w.Parent = this;

                        if (this.goal == RoomGoals.NONE) w.Open();
                        else w.Close();

                        Doors.Add(w);

                        addToGrid(i, j, w);
                    }
                    else
                    {
                        Wall w = new Wall(0, 0, this.scene, this);

                        if ((j == Rows / 2 - 1) && i == Columns && RightDoor) w.AnimationPlayer.SetCurrentAnimation(8);
                        else if ((j == Rows / 2 + 1) && i == Columns && RightDoor) w.AnimationPlayer.SetCurrentAnimation(7);
                        else if ((i == Columns / 2 - 1) && j == 0 && TopDoor) w.AnimationPlayer.SetCurrentAnimation(10);
                        else if ((i == Columns / 2 + 1) && j == 0 && TopDoor) w.AnimationPlayer.SetCurrentAnimation(9);
                        else if ((i == Columns / 2 - 1) && j == Rows && BottomDoor) w.AnimationPlayer.SetCurrentAnimation(10);
                        else if ((i == Columns / 2 + 1) && j == Rows && BottomDoor) w.AnimationPlayer.SetCurrentAnimation(9);
                        else if ((j == Rows / 2 - 1) && i == 0 && LeftDoor) w.AnimationPlayer.SetCurrentAnimation(8);
                        else if ((j == Rows / 2 + 1) && i == 0 && LeftDoor) w.AnimationPlayer.SetCurrentAnimation(7);
                        else if (i == Columns && j == Rows) w.AnimationPlayer.SetCurrentAnimation(6);
                        else if (i == 0 && j == Rows) w.AnimationPlayer.SetCurrentAnimation(5);
                        else if (i == 0 && j == 0) w.AnimationPlayer.SetCurrentAnimation(3);
                        else if (i == Columns && j == 0) w.AnimationPlayer.SetCurrentAnimation(4);
                        else if (i == 0 || i == Columns) w.AnimationPlayer.SetCurrentAnimation(2);
                        else w.AnimationPlayer.SetCurrentAnimation(1);

                        w.Parent = this;
                        addToGrid(i, j, w);
                    }
                }
            }
        }


        if (TopDoor && dir == LevelGeneratorDir.DOWN)
        {
            EntranceDoor entrance = new EntranceDoor(0, 0, this, this.scene, this);
            addToGrid(Columns / 2, 0, entrance);
        }
        ;

        if (BottomDoor && dir == LevelGeneratorDir.UP)
        {
            EntranceDoor entrance = new EntranceDoor(0, 0, this, this.scene, this);
            addToGrid(Columns / 2, Rows, entrance);
        }
        ;

        if (LeftDoor && dir == LevelGeneratorDir.RIGHT)
        {
            EntranceDoor entrance = new EntranceDoor(0, 0, this, this.scene, this);
            addToGrid(0, Rows / 2, entrance);
        }
        ;
    }
}
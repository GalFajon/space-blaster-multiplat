using System.Collections;
using System.Collections.Generic;
using GameSpecific;
using GameSpecific.LevelGenerator.Rooms;
using Microsoft.Xna.Framework;

public class Pathfinding {

    private static bool LineSegmentsIntersect(Vector2 lineOneA, Vector2 lineOneB, Vector2 lineTwoA, Vector2 lineTwoB)
    {
        return (((lineTwoB.Y - lineOneA.Y) * (lineTwoA.X - lineOneA.X) > (lineTwoA.Y - lineOneA.Y) * (lineTwoB.X - lineOneA.X)) != ((lineTwoB.Y - lineOneB.Y) * (lineTwoA.X - lineOneB.X) > (lineTwoA.Y - lineOneB.Y) * (lineTwoB.X - lineOneB.X)) && ((lineTwoA.Y - lineOneA.Y) * (lineOneB.X - lineOneA.X) > (lineOneB.Y - lineOneA.Y) * (lineTwoA.X - lineOneA.X)) != ((lineTwoB.Y - lineOneA.Y) * (lineOneB.X - lineOneA.X) > (lineOneB.Y - lineOneA.Y) * (lineTwoB.X - lineOneA.X)));
    }

    public static bool IsInCover(Vector2 soldierPos, Vector2 playerDir, Room room)
    {
        return (
            room.getGrid((int)(soldierPos.X), (int)(soldierPos.Y + playerDir.Y)) is CoverWall ||
            room.getGrid((int)(soldierPos.X + playerDir.X), (int)(soldierPos.Y)) is CoverWall
        );
    }

    public static bool HasPlayerLOS(Vector2 soldierPos, Vector2 playerPos, Room room)
    {
        for (int i = 1; i < room.Columns; i++)
        {
            for (int j = 1; j < room.Rows; j++)
            {
                var component = room.getGrid(i, j);
                if (component is Wall wall)
                {
                    var r = wall.Rect;
                    r.Offset(wall.Pos);

                    var topleft = new Vector2(r.Left, r.Top);
                    var topright = new Vector2(r.Right, r.Top);
                    var bottomleft = new Vector2(r.Left, r.Bottom);
                    var bottomright = new Vector2(r.Right, r.Bottom);

                    if (
                        LineSegmentsIntersect(soldierPos, playerPos, topleft, topright) ||
                        LineSegmentsIntersect(soldierPos, playerPos, topright, bottomright) ||
                        LineSegmentsIntersect(soldierPos, playerPos, bottomleft, bottomright) ||
                        LineSegmentsIntersect(soldierPos, playerPos, bottomleft, topleft)
                    ) return false;
                }
            }
        }

        return true;
    }

    public static Vector2 BFSFindPlayerLOS(Vector2 start, Vector2 end, Room room, Vector2 playerPos)
    {
        Queue q = new Queue();
        Dictionary<Vector2, Vector2> visited = new Dictionary<Vector2, Vector2>() { { start, start } };

        q.Enqueue(start);

        var dir = Vector2.Subtract(end, start);

        if (dir.X < 0) dir.X = -1;
        else if (dir.X > 0) dir.X = 1;

        if (dir.Y < 0) dir.Y = -1;
        else if (dir.Y > 0) dir.Y = 1;

        while (q.Count > 0)
        {
            Vector2 cur = (Vector2)q.Dequeue();

            if (
                HasPlayerLOS(room.ToWorldPos((int)cur.X, (int)cur.Y) + new Vector2(room.tileWidth / 2, room.tileHeight / 2), playerPos + new Vector2(room.tileWidth / 2, room.tileHeight / 2), room) &&
                cur.X + dir.X > 1 && cur.X + dir.X < room.Columns &&
                cur.Y + dir.Y > 1 && cur.Y + dir.Y < room.Rows
            )
            {
                return cur;
            }
            else
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (cur.X + i >= 0 && cur.Y + j >= 0 && cur.X + i <= room.Columns && cur.Y + j <= room.Rows)
                        {
                            if (i == -1 && j == -1) continue;
                            if (i == 1 && j == 1) continue;
                            if (i == -1 && j == 1) continue;
                            if (i == 1 && j == -1) continue;

                            if (room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not CoverWall && room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not Wall)
                            {
                                var vector = new Vector2(cur.X + i, cur.Y + j);
                                if (!visited.ContainsKey(vector))
                                {
                                    q.Enqueue(vector);
                                    visited[vector] = cur;
                                }
                            }
                        }
                    }
                }
            }
        }

        return start;
    }

    public static Vector2 BFSFindPlayerLOSAtDistance(Vector2 start, Vector2 end, Room room, Vector2 playerPos, float distance)
    {
        Queue q = new Queue();
        Dictionary<Vector2, Vector2> visited = new Dictionary<Vector2, Vector2>() { { start, start } };

        q.Enqueue(start);

        var dir = Vector2.Subtract(end, start);

        if (dir.X < 0) dir.X = -1;
        else if (dir.X > 0) dir.X = 1;

        if (dir.Y < 0) dir.Y = -1;
        else if (dir.Y > 0) dir.Y = 1;

        while (q.Count > 0)
        {
            Vector2 cur = (Vector2)q.Dequeue();

            if (
                HasPlayerLOS(room.ToWorldPos((int)cur.X, (int)cur.Y) + new Vector2(room.tileWidth / 2, room.tileHeight / 2), playerPos + new Vector2(room.tileWidth / 2, room.tileHeight / 2), room) &&
                Vector2.Distance(room.ToWorldPos((int)cur.X, (int)cur.Y) + new Vector2(room.tileWidth / 2, room.tileHeight / 2), playerPos + new Vector2(room.tileWidth / 2, room.tileHeight / 2)) < distance &&
                cur.X + dir.X > 1 && cur.X + dir.X < room.Columns &&
                cur.Y + dir.Y > 1 && cur.Y + dir.Y < room.Rows
            )
            {
                return cur;
            }
            else
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (cur.X + i >= 0 && cur.Y + j >= 0 && cur.X + i <= room.Columns && cur.Y + j <= room.Rows)
                        {
                            if (i == -1 && j == -1) continue;
                            if (i == 1 && j == 1) continue;
                            if (i == -1 && j == 1) continue;
                            if (i == 1 && j == -1) continue;

                            if (room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not CoverWall && room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not Wall)
                            {
                                var vector = new Vector2(cur.X + i, cur.Y + j);
                                if (!visited.ContainsKey(vector))
                                {
                                    q.Enqueue(vector);
                                    visited[vector] = cur;
                                }
                            }
                        }
                    }
                }
            }
        }

        return start;
    }

    public static Vector2 BFSFindCoverWithLOS(Vector2 start, Vector2 end, Room room, Vector2 playerPos)
    {
        Queue q = new Queue();
        Dictionary<Vector2, Vector2> visited = new Dictionary<Vector2, Vector2>() { { start, start } };

        q.Enqueue(start);

        var dir = Vector2.Subtract(end, start);

        if (dir.X < 0) dir.X = -1;
        else if (dir.X > 0) dir.X = 1;

        if (dir.Y < 0) dir.Y = -1;
        else if (dir.Y > 0) dir.Y = 1;

        while (q.Count > 0)
        {
            Vector2 cur = (Vector2)q.Dequeue();

            if (
                (
                    (
                        room.getGrid((int)(cur.X), (int)(cur.Y + dir.Y)) is CoverWall ||
                        room.getGrid((int)(cur.X + dir.X), (int)(cur.Y)) is CoverWall
                    ) &&
                    HasPlayerLOS(room.ToWorldPos((int)cur.X, (int)cur.Y) + new Vector2(room.tileWidth / 2, room.tileHeight / 2), playerPos + new Vector2(room.tileWidth / 2, room.tileHeight / 2), room)

                ) &&
                cur.X + dir.X > 1 && cur.X + dir.X < room.Columns &&
                cur.Y + dir.Y > 1 && cur.Y + dir.Y < room.Rows
            )
            {
                return cur;
            }
            else
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (cur.X + i >= 0 && cur.Y + j >= 0 && cur.X + i <= room.Columns && cur.Y + j <= room.Rows)
                        {
                            if (i == -1 && j == -1) continue;
                            if (i == 1 && j == 1) continue;
                            if (i == -1 && j == 1) continue;
                            if (i == 1 && j == -1) continue;

                            if (room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not CoverWall && room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not Wall)
                            {
                                var vector = new Vector2(cur.X + i, cur.Y + j);
                                if (!visited.ContainsKey(vector))
                                {
                                    q.Enqueue(vector);
                                    visited[vector] = cur;
                                }
                            }
                        }
                    }
                }
            }
        }

        return start;
    }

    public static Dictionary<Vector2, Vector2> BFS(Vector2 start, Vector2 end, Room room)
    {
        Queue q = new Queue();
        Dictionary<Vector2, Vector2> visited = new Dictionary<Vector2, Vector2>() { { start, start } };

        q.Enqueue(start);

        while (q.Count > 0)
        {
            Vector2 cur = (Vector2)q.Dequeue();

            if (cur == end) break;
            else
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (cur.X + i >= 0 && cur.Y + j >= 0 && cur.X + i <= room.Columns && cur.Y + j <= room.Rows)
                        {
                            if (i == -1 && j == -1) continue;
                            if (i == 1 && j == 1) continue;
                            if (i == -1 && j == 1) continue;
                            if (i == 1 && j == -1) continue;

                            if (room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not Wall && room.getGrid((int)(cur.X + i), (int)(cur.Y + j)) is not CoverWall)
                            {
                                var vector = new Vector2(cur.X + i, cur.Y + j);
                                if (!visited.ContainsKey(vector))
                                {
                                    q.Enqueue(vector);
                                    visited[vector] = cur;
                                }
                            }
                        }
                    }
                }
            }
        }

        return visited;
    }
}
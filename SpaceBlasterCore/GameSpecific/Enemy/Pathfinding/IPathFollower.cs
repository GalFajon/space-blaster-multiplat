using System.Collections.Generic;
using GameSpecific;
using GameSpecific.LevelGenerator.Rooms;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster.Scene;

public interface IPathFollower
{
    double MoveTimer { get; set; }
    double MaxMoveTimer { get; set; }
    Stack<Vector2> Path { get; set; }
    Room Room { get; set; }
    List<Marker> Markers { get; set; }

    public void ClearMarkers()
    {
        //foreach (var marker in Markers) marker.Destroy();
        //Markers.Clear();
    }

    public void GenerateMarkers(SpaceBlaster.Scene.Scene scene)
    {
        /*foreach (var marker in Markers) marker.Destroy();
        Markers.Clear();

        foreach (var path in Path)
        {
            var p = Room.ToWorldPos((int)path.X, (int)path.Y);
            var m = new Marker(p.X, p.Y, scene, null);
            Markers.Add(m);
            scene.Spawn(m);
        }*/
    }

    public void MoveAlongPath(GameTime gameTime)
    {
        // this is extremely hacky, rework the interface system ASAP
        if (Path.Count > 0 && this is Position positioned)
        {
            var p = Room.ToWorldPos((int)Path.Peek().X, (int)Path.Peek().Y);
            var worldPos = new Vector2(p.X, p.Y);

            if (Vector2.Distance(positioned.Pos, worldPos) > 1)
            {
                MoveTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (MoveTimer > MaxMoveTimer) MoveTimer = MaxMoveTimer;
                positioned.Pos = Vector2.Lerp(positioned.LocalPos, worldPos - Room.Pos, (float)(MoveTimer / MaxMoveTimer));
            }
            else if (Path.Count > 0)
            {
                positioned.Pos = new Vector2(Path.Peek().X * Room.tileWidth, Path.Peek().Y * Room.tileHeight);
                Path.Pop();
                MoveTimer = 0;
            }
        }
    }
}
using Microsoft.Xna.Framework;
namespace GameSpecific;
using System.Collections.Generic;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;

public enum EnemyZombieState
{
    STILL,
    MOVING
};

public class EnemyZombie : Enemy, IPathFollower, IAnimatable
{
    public double MoveTimer { get; set; } = 0;
    public double MaxMoveTimer { get; set; } = 1250;
    public Stack<Vector2> Path { get; set; } = new Stack<Vector2>();
    public List<Marker> Markers { get; set; } = new List<Marker>();

    private double shootTimer = 400;
    private double currentTimer = 0;
    public EnemyZombieState current = EnemyZombieState.STILL;

    public EnemyZombie(float x, float y, Scene scene, SceneObject parent = null) : base(5, x, y, new Rectangle(0, 0, 32, 32), true, scene, parent)
    {
        this.currencyValue = 5;
        this.StunTimer = 100;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(88, 3), 2, 0.2, 16, 13, 3, 0f, new Vector2(0, 0)) },
                { 1,  new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(89, 21), 4, 0.2, 16, 13, 3, 0f, new Vector2(0, 0)) }
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
    }

    public override void HandleAI(GameTime gameTime)
    {
        base.HandleAI(gameTime);
        if (this.IsHit) return;

        if (this.scene is Level l)
        {
            Vector2 playerPos = Room.getTile(l._player.Pos);

            if (playerPos.X > 0 && playerPos.Y > 0 && playerPos.X < Room.Columns && playerPos.Y < Room.Rows)
            {
                if (Vector2.Distance(l._player.Pos, this.Pos) < 200)
                {
                    current = EnemyZombieState.STILL;
                    this.AnimationPlayer.SetCurrentAnimation(0);
                    this.Speed = 0;

                    if (currentTimer > shootTimer)
                    {
                        SoundEffectsManager.Play(this, "enemy_spots_player");
                        var PlayerDir = Vector2.Normalize(Vector2.Subtract(l._player.Pos, this.Pos));
                        this.scene.Spawn(new Projectile(1, this.Pos.X + 16, this.Pos.Y + 16, PlayerDir, 300, null));
                        currentTimer = 0;
                    }

                    currentTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    if (Path.Count > 0)
                    {
                        this.AnimationPlayer.SetCurrentAnimation(1);
                        current = EnemyZombieState.MOVING;
                        (this as IPathFollower).MoveAlongPath(gameTime);
                    }
                    else
                    {
                        (this as IPathFollower).ClearMarkers();

                        Vector2 zombPos = Room.getTile(this.Pos);
                        var goal = Pathfinding.BFSFindPlayerLOSAtDistance(zombPos, playerPos, this.Room, l._player.Pos, 200);
                        var path = Pathfinding.BFS(zombPos, goal, Room);

                        if (path.Keys.Count > 0)
                        {
                            Vector2 start = goal;
                            Path.Push(goal);

                            while (path.ContainsKey(start) && path[start] != zombPos)
                            {
                                start = path[start];
                                Path.Push(start);
                            }

                            this.Pos = new Vector2(path[start].X * Room.tileWidth, path[start].Y * Room.tileHeight);

                            Path.Push(zombPos);
                        }

                        (this as IPathFollower).GenerateMarkers(this.scene);
                    }
                }
            }
        }
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is not Projectile) base.HandleCollision(collided);
        if (this.Health <= 0) (this as IPathFollower).ClearMarkers();
    }
}
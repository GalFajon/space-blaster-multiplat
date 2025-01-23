using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Gameplay;
using GameEngine.Gameplay.Audio;

namespace GameSpecific;

public class EnemyBee : Enemy, IPathFollower, IAnimatable
{
    public double MoveTimer { get; set; } = 0;
    public double MaxMoveTimer { get; set; } = 1000;
    public Stack<Vector2> Path { get; set; } = new Stack<Vector2>();
    public List<Marker> Markers { get; set; } = new List<Marker>();
    public double ShootTimer = 1000;
    public double CurrentTimer = 0;
    public EnemyBee(float x, float y, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(3, x, y, new Rectangle(0, 0, 32, 32), true, scene, parent)
    {
        this.currencyValue = 5;
        this.IsSolid = false;
        this.StunTimer = 300;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                    { 0, new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(0, 114), 8, 0.2, 16, 16, 3, 0f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
    }

    public override void HandleAI(GameTime gameTime)
    {
        base.HandleAI(gameTime);
        if (this.IsHit) return;

        var spr = this.AnimationPlayer.GetCurrentSprite();
        if (this.Direction.X < 0) spr.FlipH();
        else spr.Unflip();


        if (this.scene is Level l)
        {
            Vector2 playerPos = Room.getTile(l._player.Pos);

            if (playerPos.X > 0 && playerPos.Y > 0 && playerPos.X < Room.Columns && playerPos.Y < Room.Rows)
            {
                if (Path.Count > 0) (this as IPathFollower).MoveAlongPath(gameTime);
                else
                {
                    (this as IPathFollower).ClearMarkers();

                    Vector2 beePos = Room.getTile(this.Pos);
                    var goal = new Vector2(RandomNumber.GetRandomNumber(1, this.Room.Columns - 1), RandomNumber.GetRandomNumber(1, this.Room.Rows - 1));
                    var path = Pathfinding.BFS(beePos, goal, Room);

                    if (path.Keys.Count > 0)
                    {
                        Vector2 start = goal;
                        Path.Push(goal);

                        while (path.ContainsKey(start) && path[start] != beePos)
                        {
                            start = path[start];
                            Path.Push(start);
                        }

                        Path.Push(beePos);
                    }

                    (this as IPathFollower).GenerateMarkers(this.scene);
                }

                if (CurrentTimer > ShootTimer)
                {
                    SoundEffectsManager.Play(this, "enemybee_fires");

                    this.scene.Spawn(new Projectile(1, this.Pos.X + 16, this.Pos.Y + 16, new Vector2(1, 1), 100, null));
                    this.scene.Spawn(new Projectile(1, this.Pos.X + 16, this.Pos.Y + 16, new Vector2(-1, 1), 100, null));
                    this.scene.Spawn(new Projectile(1, this.Pos.X + 16, this.Pos.Y + 16, new Vector2(1, -1), 100, null));
                    this.scene.Spawn(new Projectile(1, this.Pos.X + 16, this.Pos.Y + 16, new Vector2(-1, -1), 100, null));
                    CurrentTimer = 0;
                }

                CurrentTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is not Projectile) base.HandleCollision(collided);

        if (this.Health <= 0) (this as IPathFollower).ClearMarkers();
    }
}
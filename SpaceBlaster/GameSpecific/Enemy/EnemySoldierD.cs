using Microsoft.Xna.Framework;
namespace GameSpecific;

using System.Collections.Generic;
using System.Diagnostics;
using GameEngine.Graphics;
using GameEngine.Scene.Components;
using GameSpecific.LevelGenerator.Rooms;

public enum EnemySoldierState
{
    NOT_IN_COVER,
    MOVING_TO_COVER,
    FINISHED_MOVING,
    IN_COVER,
    SHOOTING
}

public class EnemySoldierD : Enemy, IPathFollower, IAnimatable
{

    public EnemySoldierState currentState = EnemySoldierState.NOT_IN_COVER;
    private Dictionary<Vector2, Vector2> path = null;
    private double CoverTimer = 0;
    private double OutOfCoverTimer = 1000;
    private double InCoverTimer = 500;
    public double MoveTimer { get; set; } = 0;
    public double MaxMoveTimer { get; set; } = 1700;
    public Stack<Vector2> Path { get; set; } = new Stack<Vector2>();
    public List<Marker> Markers { get; set; } = new List<Marker>();
    private Weapon weapon;
    public EnemySoldierD(WeaponStats weaponstats, float x, float y, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(4, x, y, new Rectangle(0, 0, 48, 48), true, scene, parent)
    {
        this.Health = 3;
        this.MaxHealth = 3;
        this.currencyValue = 10;
        this.IsSolid = false;
        weapon = new Weapon(weaponstats, 24, 24, this.scene, this);
        this.scene.Spawn(weapon);

        this.StunTimer = 150;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(0, 0), 2, 0.2, 16, 16, 3, 0.05f, new Vector2(0, 0)) },
                { 1, new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(0, 16), 2, 0.2, 16, 16, 3, 0.05f, new Vector2(0, 0)) },
                { 2, new AnimatedSprite(SpaceBlaster.SpaceBlasterGame.TextureAtlas, new Vector2(3, 44), 1, 0.2, 13, 13, 3, 0.05f, new Vector2(0, 0)) }
            }
        );
    }

    public void Shoot()
    {
        this.weapon.Aim(Vector2.Normalize(Vector2.Subtract(((Level)this.scene)._player.Pos, this.Pos)));
        this.weapon.Shoot();
    }

    public override void HandleAI(GameTime gameTime)
    {
        base.HandleAI(gameTime);

        if (this.scene is Level l && !this.IsHit)
        {
            Vector2 playerPos = Room.getTile(l._player.Pos);
            Vector2 soldierPos = Room.getTile(this.Pos);
            Vector2 playerDir = Vector2.Subtract(playerPos, soldierPos);

            if (playerPos.X > 0 && playerPos.Y > 0 && playerPos.X < Room.Columns && playerPos.Y < Room.Rows)
            {
                if (playerDir.X < 0) playerDir.X = -1;
                else if (playerDir.X > 0) playerDir.X = 1;

                if (playerDir.Y < 0) playerDir.Y = -1;
                else if (playerDir.Y > 0) playerDir.Y = 1;

                if (this.currentState != EnemySoldierState.MOVING_TO_COVER && this.currentState != EnemySoldierState.SHOOTING)
                {
                    if (Pathfinding.IsInCover(soldierPos, playerDir, this.Room) && Pathfinding.HasPlayerLOS(this.Pos, l._player.Pos + l._player.Rect.Center.ToVector2(), this.Room)) this.currentState = EnemySoldierState.IN_COVER;
                    else if (Pathfinding.HasPlayerLOS(this.Pos, l._player.Pos + l._player.Rect.Center.ToVector2(), this.Room)) this.currentState = EnemySoldierState.SHOOTING;
                    else this.currentState = EnemySoldierState.NOT_IN_COVER;
                }

                if (currentState == EnemySoldierState.NOT_IN_COVER)
                {
                    this.AnimationPlayer.SetCurrentAnimation(0);

                    Path.Clear();

                    var goal = Pathfinding.BFSFindCoverWithLOS(soldierPos, playerPos, Room, l._player.Pos);
                    if (goal == soldierPos) goal = Pathfinding.BFSFindPlayerLOS(soldierPos, playerPos, Room, l._player.Pos);

                    path = Pathfinding.BFS(soldierPos, goal, Room);

                    if (path.Keys.Count > 0)
                    {
                        Vector2 start = goal;
                        Path.Push(goal);

                        while (path.ContainsKey(start) && path[start] != soldierPos)
                        {
                            start = path[start];
                            Path.Push(start);
                        }

                        this.Pos = new Vector2(path[start].X * Room.tileWidth, path[start].Y * Room.tileHeight);

                        Path.Push(soldierPos);
                    }

                    this.currentState = EnemySoldierState.MOVING_TO_COVER;
                }
                else if (currentState == EnemySoldierState.MOVING_TO_COVER)
                {
                    this.AnimationPlayer.SetCurrentAnimation(1);

                    if (Vector2.Distance(this.Pos, l._player.Pos) < 250) this.Shoot();

                    if (Path.Count > 0) (this as IPathFollower).MoveAlongPath(gameTime);
                    else this.currentState = EnemySoldierState.FINISHED_MOVING;

                }
                else if (currentState == EnemySoldierState.FINISHED_MOVING)
                {
                    this.AnimationPlayer.SetCurrentAnimation(0);
                }
                else if (currentState == EnemySoldierState.IN_COVER)
                {
                    this.AnimationPlayer.SetCurrentAnimation(2);

                    this.CoverTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (this.CoverTimer > this.InCoverTimer)
                    {
                        this.CoverTimer = 0;
                        this.currentState = EnemySoldierState.SHOOTING;
                    }
                }
                else if (currentState == EnemySoldierState.SHOOTING)
                {
                    this.AnimationPlayer.SetCurrentAnimation(0);

                    this.CoverTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    this.Shoot();

                    if (this.CoverTimer > this.OutOfCoverTimer)
                    {
                        this.CoverTimer = 0;
                        this.currentState = EnemySoldierState.IN_COVER;
                    }
                }
            }
        }
    }

    public override void HandleCollision(Collider collided)
    {
        if ((collided is PlayerProjectile || collided is DamageArea) && this.currentState != EnemySoldierState.IN_COVER && !this.IsHit)
        {
            if (collided is PlayerProjectile p)
            {
                this.Health -= p.Damage;
                this.IsHit = true;
                this.CurStunTimer = 0;
            }

            if (collided is DamageArea d && d.active)
            {
                this.Health -= d.Damage;
                this.IsHit = true;
                this.CurStunTimer = 0;
            }
        }
    }
}
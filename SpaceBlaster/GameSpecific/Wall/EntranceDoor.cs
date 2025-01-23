using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using GameSpecific.LevelGenerator.Rooms;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;

public enum EntranceDoorState
{
    WAITING,
    PASSED_THROUGH,
    CLOSED
}

public class EntranceDoor : Collider, IAnimatable, IArtificialIntelligence
{
    public AnimationPlayer AnimationPlayer { get; set; }
    private EntranceDoorState current;
    public Room Room;
    public EntranceDoor(float x, float y, Room Room, Scene scene, SceneObject parent = null) : base(x, y, new Rectangle(0, 0, 32, 32), scene, parent)
    {
        this.Room = Room;

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(52, 133), 1, 0.2, 16, 16, 3, 0f) },
                { 1, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(70, 133), 1, 0.2, 16, 16, 3, 0f) }
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(1);
        this.IsSolid = false;
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        if (current == EntranceDoorState.PASSED_THROUGH)
        {
            Level l = (Level)this.scene;
            var top = this.Room.getTile(l._player.Pos + new Vector2(l._player.Rect.Center.ToVector2().X, l._player.Rect.Top));
            var bottom = this.Room.getTile(l._player.Pos + new Vector2(l._player.Rect.Center.ToVector2().X, l._player.Rect.Bottom));
            var p2 = this.Room.getTile(l._player.Pos);

            if (
                p2.X > 0 && p2.X < this.Room.Columns &&
                top.Y > 0 && top.Y < this.Room.Rows &&
                bottom.Y > 0 && bottom.Y < this.Room.Rows
            )
            {
                this.IsSolid = true;
                this.AnimationPlayer.SetCurrentAnimation(0);
                current = EntranceDoorState.CLOSED;
            }
        }
    }

    public override void HandleCollision(Collider collided)
    {
        if (collided is not Player) base.HandleCollision(collided);
        else
        {
            if (current == EntranceDoorState.WAITING) current = EntranceDoorState.PASSED_THROUGH;
            else if (current == EntranceDoorState.CLOSED) base.HandleCollision(collided);
        }
    }
}
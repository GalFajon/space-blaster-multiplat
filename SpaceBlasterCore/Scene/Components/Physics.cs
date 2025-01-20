using Microsoft.Xna.Framework;
namespace Scene.Components;

// this can be separated into MOVABLE and PHYSICS interfaces (Movable is for objects that need a direction,speed,velocity etc. but cannot collide)
public abstract class Physics : Collider
{

    public Vector2 Velocity { get; set; } = new Vector2(0, 0);
    public Vector2 Direction { get; set; } = new Vector2(0, 0);
    public float Speed { get; set; } = 0;

    public Physics(float x, float y, Rectangle rect, bool canCollide, SpaceBlaster.Scene.Scene scene, SceneObject parent) : base(x, y, rect, scene, parent)
    {
        this.Enabled = canCollide;
        this.Pos = new Vector2(x, y);
    }
}
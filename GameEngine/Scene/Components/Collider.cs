using Microsoft.Xna.Framework;
namespace GameEngine.Scene.Components;

public abstract class Collider : Position
{
    public Rectangle Rect { get; set; }
    public bool Enabled { get; set; } = true;
    public bool Colliding { get; set; } = false;
    public bool IsSolid = true;

    public Collider(float x, float y, Rectangle rect, GameEngine.Scene.Scene scene, SceneObject parent) : base(x, y, scene, parent)
    {
        this.Rect = rect;
        this.Pos = new Vector2(x, y);
    }

    public static bool DetectCollision(Collider c1, Collider c2)
    {
        if (c1.Enabled && c2.Enabled)
        {
            Rectangle r1 = c1.Rect;
            Rectangle r2 = c2.Rect;

            r1.Offset(c1.Pos);
            r2.Offset(c2.Pos);

            if (r1.Intersects(r2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }

    public virtual void HandleCollision(Collider collided) { return; }
}
using Microsoft.Xna.Framework;
namespace GameEngine.Scene.Components;

public abstract class Physics : Collider
{

    public Vector2 Velocity { get; set; } = new Vector2(0, 0);
    public Vector2 Direction { get; set; } = new Vector2(0, 0);
    public float Speed { get; set; } = 0;

    public Physics(float x, float y, Rectangle rect, bool canCollide, GameEngine.Scene.Scene scene, SceneObject parent) : base(x, y, rect, scene, parent)
    {
        this.Enabled = canCollide;
        this.Pos = new Vector2(x, y);
    }
}
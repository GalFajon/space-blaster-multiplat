using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene.Components;

public class Camera : Position
{
    public float Rot = 0;

    private float scaleX = 1.0f;
    private float scaleY = 1.0f;
    public float ScaleX
    {
        get
        {
            return scaleX;
        }
        set
        {
            scaleX = value;
        }
    }
    public float ScaleY
    {
        get
        {
            return scaleY;
        }
        set
        {
            scaleY = value;
        }
    }

    public Camera(float x, float y, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.Pos = new Vector2(x, y);
    }
}
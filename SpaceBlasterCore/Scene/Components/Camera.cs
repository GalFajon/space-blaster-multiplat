using Microsoft.Xna.Framework;
namespace Scene.Components;
public class Camera : Position
{
    public float Rot = 0;
    public float ScaleX = 1f;
    public float ScaleY = 1f;

    public Camera(float x, float y, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.Pos = new Vector2(x, y);
    }
}
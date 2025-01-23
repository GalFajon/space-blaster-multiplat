using Microsoft.Xna.Framework;
namespace GameEngine.Scene.Components;

public abstract class Position : SceneObject
{
    private Vector2 _pos;
    public Vector2 Pos
    {
        get
        {
            if (this.Parent != null && this.Parent is Position p) return Vector2.Add(p.Pos, _pos);
            else return _pos;
        }
        set
        {
            _pos = value;
        }
    }

    public Vector2 LocalPos
    {
        get
        {
            return _pos;
        }
        set
        {
            _pos = value;
        }
    }

    public Position(float x, float y, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(scene, parent)
    {
        this.Pos = new Vector2(x, y);
    }
}
using Microsoft.Xna.Framework;
using GameEngine.Scene.Components;

namespace GameEngine.Scene.UI;

public class Label : Position
{
    public string Text = "";
    public Color Color = Color.White;
    public int Scale = 1;
    public Label(float x, float y, string text, Color color, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        Text = text;
        Color = color;
    }
}
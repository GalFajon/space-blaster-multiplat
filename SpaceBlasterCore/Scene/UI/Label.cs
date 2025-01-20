using Scene.Components;
using Microsoft.Xna.Framework;

public class Label : Position
{
    public string Text = "";
    public Color Color = Color.White;

    public Label(float x, float y, string text, Color color, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        Text = text;
        Color = color;
    }
}
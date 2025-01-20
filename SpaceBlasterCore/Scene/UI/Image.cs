using Scene.Components;
using Microsoft.Xna.Framework;

using SpaceBlaster.Graphics;

public class Image : Position
{
    public Sprite Sprite { get; set; }
    public Image(float x, float y, Sprite sprite, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        Sprite = sprite;
    }
}
using Microsoft.Xna.Framework;
using GameEngine.Scene.Components;
using GameEngine.Graphics;

namespace GameEngine.Scene.UI;

public class Image : Position
{
    public Sprite Sprite { get; set; }
    public Image(float x, float y, Sprite sprite, GameEngine.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        Sprite = sprite;
    }
}
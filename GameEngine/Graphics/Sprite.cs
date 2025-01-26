using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics;

public class Sprite
{
    public Texture2D texture;
    public Rectangle rect;
    public Vector2 origin;
    public SpriteEffects effect = SpriteEffects.None;
    public float Scale = 1;
    public float Rotation = 0;
    public float depth = 0.0f;
    public Color color = Color.White;
    public Vector2 center = Vector2.Zero;
    public float opacity = 1f;

    public Sprite(Texture2D texture, Rectangle rect, Vector2 origin, float scale = 1)
    {
        this.texture = texture;
        this.rect = rect;
        this.origin = origin;
        this.Scale = scale;
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameEngine.Graphics;

public class AnimatedSprite
{
    public Texture2D texture;
    public Vector2 origin;

    public List<Rectangle> rects = new List<Rectangle>();
    public int Width = 0;
    public int Height = 0;
    public float Rotation = 0;
    public int Scale = 1;
    private int frames;
    public int currentFrame = 0;
    private double frameTime;
    private double frameTimeLeft;
    private bool playing = true;
    public float depth = 0.0f;
    public Color color = Color.White;
    public bool loop = true;
    public bool finished = false;

    public SpriteEffects effect = SpriteEffects.None;

    public AnimatedSprite(Texture2D texture, Vector2 origin, int frames, double time, int frameWidth, int frameHeight, int scale, float depth)
    {
        this.texture = texture;
        this.frames = frames;
        this.origin = origin;
        this.Scale = scale;

        Width = frameWidth;
        Height = frameHeight;

        frameTime = time;
        frameTimeLeft = time;

        this.depth = depth;

        for (int i = 0; i < frames; i++) rects.Add(new Rectangle((int)origin.X + (i * frameWidth), (int)origin.Y, frameWidth, frameHeight));
    }

    public void Unflip()
    {
        this.effect = SpriteEffects.None;
    }

    public void FlipH()
    {
        this.effect = SpriteEffects.FlipHorizontally;
    }

    public void FlipV()
    {
        this.effect = SpriteEffects.FlipVertically;
    }

    public void Play()
    {
        this.playing = true;
    }

    public void Pause()
    {
        this.playing = false;
    }

    public void Reset()
    {
        currentFrame = 0;
        frameTimeLeft = frameTime;
    }

    public void Update(GameTime gameTime)
    {
        if (playing)
        {
            frameTimeLeft -= gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimeLeft <= 0)
            {
                frameTimeLeft = frameTime;
                if (this.loop || (currentFrame != frames - 1)) currentFrame = (currentFrame + 1) % frames;

                if (!this.loop && currentFrame == frames - 1) this.finished = true;
                else this.finished = false;
            }
        }
    }
}
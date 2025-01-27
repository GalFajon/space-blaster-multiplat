using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;

public class Particle : Physics, GameEngine.Scene.Components.IDrawable, IArtificialIntelligence
{
    public Sprite sprite { get; set; }
    public double lifespan = 1f;

    public float opacityStart = 1f;
    public float opacityEnd = 0f;

    public float scaleStart = 1f;
    public float scaleEnd = 0f;

    public Color colorStart = Color.White;
    public Color colorEnd = Color.White;

    private double lifespanTimer;
    private float lifespanAmount;

    private float currentOpacity;
    private float currentScale;
    private Color currentColor;

    private float initialScale = 1f;

    public Particle(int x, int y, Sprite sprite, float maxLifespan, float opacityStart, float  opacityEnd, float scaleStart, float scaleEnd, Color colorStart, Color colorEnd, Scene scene, SceneObject parent) : base(x,y, new Rectangle(0,0,0,0), false,scene, parent)
    {
        initialScale = sprite.Scale;
        this.sprite = new Sprite(sprite.texture,sprite.rect,sprite.origin,sprite.Scale);
        this.sprite.color = colorStart; 

        this.lifespan = maxLifespan;
        this.lifespanTimer = 0;

        this.opacityStart = opacityStart;
        this.opacityEnd = opacityEnd;

        this.scaleStart = scaleStart;
        this.scaleEnd = scaleEnd;

        this.colorStart = colorStart;
        this.colorEnd = colorEnd;
    }

    public void HandleAI(GameTime gameTime)
    {
        lifespanTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (lifespanTimer > lifespan) this.Destroy();

        lifespanAmount = MathHelper.Clamp((float)(lifespanTimer / lifespan),0,1);
        currentOpacity = MathHelper.Clamp(MathHelper.Lerp(opacityStart, opacityEnd, lifespanAmount), 0, 1);
        currentScale = MathHelper.Clamp(MathHelper.Lerp(scaleStart * initialScale, scaleEnd * initialScale, lifespanAmount),scaleStart * initialScale,scaleEnd * initialScale);
        currentColor = Color.Lerp(colorStart, colorEnd, lifespanAmount);

        sprite.opacity = currentOpacity;
        sprite.color = currentColor;
        sprite.Scale = currentScale;
    }
}
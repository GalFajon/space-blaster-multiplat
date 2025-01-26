using GameEngine.Gameplay;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;

public class ParticleEmitter : Position, IArtificialIntelligence
{
    public Sprite sprite { get; set; }
    public float lifespan = 1f;

    public float opacityStart = 1f;
    public float opacityEnd = 0f;

    public float scaleStart = 1f;
    public float scaleEnd = 1f;

    public Color colorStart = Color.White;
    public Color colorEnd = Color.White;

    public bool oneShot = false;
    public double spawnInterval = 500;
    public double currentTimer = 0;

    public int minSpeed = 0;
    public int maxSpeed = 100;

    public Vector2 dir = new Vector2(0,0);
    
    public float spread = 0f;

    public int howMany = 1;
    public bool emit = true;

    public ParticleEmitter(int x, int y, Sprite sprite, Vector2 dir, float spread, int minSpeed, int maxSpeed, int howMany, bool oneShot, float spawnInterval, float maxLifespan, float opacityStart, float opacityEnd, float scaleStart, float scaleEnd, Color startColor, Color endColor, Scene scene, SceneObject parent) : base(x, y, scene, parent)
    {
        this.sprite = sprite;
        this.lifespan = maxLifespan;
        this.opacityStart = opacityStart;
        this.opacityEnd = opacityEnd;
        this.oneShot = oneShot;
        this.spawnInterval = spawnInterval;
        this.howMany = howMany;
        this.dir = dir;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.spread = spread;
        this.scaleStart = scaleStart;
        this.scaleEnd = scaleEnd;
        this.colorStart = startColor;
        this.colorEnd = endColor;
    }

    public void HandleAI(GameTime gameTime)
    {
        if (this.emit)
        {
            currentTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (currentTimer > spawnInterval)
            {
                for (int i = 0; i < howMany; i++)
                {
                    var p = new Particle(0, 0, this.sprite, this.lifespan, this.opacityStart, this.opacityEnd, this.scaleStart, this.scaleEnd, this.colorStart, this.colorEnd, this.scene, this);
                    p.IsSolid = false;

                    p.Direction = Vector2.Rotate(dir, spread * RandomNumber.GetRandomFloat(-1, 1));
                    p.Speed = RandomNumber.GetRandomNumber(this.minSpeed, this.maxSpeed);

                    this.scene.Spawn(p);
                }

                currentTimer = 0;
            }
        }
        else this.currentTimer = 0;
    }
}
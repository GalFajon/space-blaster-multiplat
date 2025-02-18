using Microsoft.Xna.Framework;
namespace GameSpecific;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using GameSpecific.LevelGenerator.Rooms;

public class Enemy : Physics, IArtificialIntelligence, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    protected double StunTimer = 300;
    protected double CurStunTimer = 0;
    public bool IsHit = false;
    protected bool DoesStun = true;
    public Room Room { get; set; }
    protected Bar healthBar = new Bar(0, -20, null);
    protected float MaxHealth = 4;
    protected float health = 4;
    protected int currencyValue = 10;
    protected bool damagingExplosion = false;
    protected float Health
    {
        get => health;
        set
        {
            health = value;

            if (health <= MaxHealth && health > MaxHealth / 2) healthBar.SetState(BarState.HIGH);
            else if (health <= 2) healthBar.SetState(BarState.LOW);
            else if (health <= MaxHealth / 2) healthBar.SetState(BarState.MEDIUM);

            if (health <= 0)
            {
                this.Destroy();
                int s = damagingExplosion ? 12 : 3;
                var e = new Explosion(this.Pos.X - this.Rect.Center.X * (damagingExplosion ? 4 : 1), this.Pos.Y - this.Rect.Center.Y * (damagingExplosion ? 4 : 1), this.scene, damagingExplosion, s);
                
                this.scene.Spawn(e);

                if (this.currencyValue > 0)
                {
                    var c = new Currency(this.currencyValue, this.Pos.X, this.Pos.Y, this.scene, null);
                    this.scene.Spawn(c);
                }

                this.Room.removeFromGrid(this);
                this.Room.UpdateGoal();
            }
        }
    }

    public Enemy(int health, float x, float y, Rectangle rect, bool canCollide, Scene scene, SceneObject parent = null) : base(x, y, rect, canCollide, scene, parent)
    {
        this.MaxHealth = health;
        this.Health = health;

        healthBar.Parent = this;
        this.scene.Spawn(healthBar);
        healthBar.SetState(BarState.HIGH);
    }

    public override void HandleCollision(Collider collided)
    {
        if ((collided is PlayerProjectile || collided is DamageArea) && !this.IsHit)
        {
            if (collided is PlayerProjectile p)
            {
                this.Health -= p.Damage;
                this.IsHit = true;
                this.CurStunTimer = 0;
            }
            else if (collided is DamageArea d && d.active)
            {
                this.Health -= d.Damage;
                this.IsHit = true;
                this.CurStunTimer = 0;
            }
        }
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        if (this.IsHit && this.DoesStun)
        {
            this.AnimationPlayer.GetCurrentSprite().Pause();
            this.AnimationPlayer.GetCurrentSprite().color = Color.Red;

            this.Speed = 0;
            this.CurStunTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.CurStunTimer > this.StunTimer) this.IsHit = false;
            else return;
        }
        else
        {
            this.AnimationPlayer.GetCurrentSprite().color = Color.White;
            this.AnimationPlayer.GetCurrentSprite().Play();
        }
    }
}
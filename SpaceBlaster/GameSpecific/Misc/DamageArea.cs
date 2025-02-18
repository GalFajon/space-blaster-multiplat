using System.Collections.Generic;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;

public class DamageArea : Collider
{
    public float Damage = 1;
    public bool destroyOnCollision = false;
    public bool active = false;
    public DamageArea(float x, float y, int width, int height, Scene scene, SceneObject parent = null, bool a = false) : base(x, y, new Rectangle(0, 0, width, height), scene, parent) {
        this.IsSolid = false;
        this.active = a;
    }

    public override void HandleCollision(Collider collider)
    {
        if (destroyOnCollision) this.Destroy();
    }
}
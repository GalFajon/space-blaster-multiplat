using System.Collections.Generic;
using GameEngine.Gameplay.Audio;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using Microsoft.Xna.Framework;
using SpaceBlaster;
namespace GameSpecific;


public class DamageWall : Position, IAnimatable
{
    private DamageArea damageArea = null;
    public AnimationPlayer AnimationPlayer { get; set; }
    public DamageWall(float x, float y, Scene scene) : base(x, y, scene)
    {
        var s = new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(107, 51), 1, 0.1, 16, 16, 3, 0f, new Vector2(0, 0));

        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, s },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
 
        damageArea = new DamageArea(0, 0, 16 * 3, 16 * 3, this.scene, this, true);
        damageArea.Damage = 1;
        this.scene.Spawn(damageArea);
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster;
namespace GameSpecific;

public class BackgroundWall : Position, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public BackgroundWall(float x, float y, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(124, 34), 1, 0.2, 16, 16, 3, 0.1f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);

        this.Pos = new Vector2(x, y);
    }
}
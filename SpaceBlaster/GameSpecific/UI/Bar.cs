using Microsoft.Xna.Framework;
namespace GameSpecific;

using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using SpaceBlaster;

public enum BarState
{
    LOW,
    MEDIUM,
    HIGH
}

public class Bar : Position, IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public BarState CurrentState;
    public Bar(float x, float y, Scene scene) : base(x, y, scene)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(32, 132), 1, 0.2, 16, 4, 3, 0f) },
                { 1, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(32, 136), 1, 0.2, 16, 4, 3, 0f) },
                { 2, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(32, 140), 1, 0.2, 16, 4, 3, 0f) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
    }

    public void SetState(BarState s)
    {
        if (s == BarState.HIGH) this.AnimationPlayer.SetCurrentAnimation(0);
        if (s == BarState.MEDIUM) this.AnimationPlayer.SetCurrentAnimation(1);
        if (s == BarState.LOW) this.AnimationPlayer.SetCurrentAnimation(2);

        this.CurrentState = s;
    }

}
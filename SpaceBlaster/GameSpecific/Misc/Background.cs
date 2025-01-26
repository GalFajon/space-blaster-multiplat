using SpaceBlaster;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Gameplay.Audio;

public class Background : Position, IUIAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public Background(float x, float y, Scene scene) : base(x, y, scene)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.Background, new Vector2(0, 0), 1, 0.1, 480, 270, 4, 1f, new Vector2(0, 0)) },
                { 1, new AnimatedSprite(SpaceBlasterGame.BackgroundTitle, new Vector2(0, 0), 1, 0.1, 480, 270, 4, 1f, new Vector2(0, 0)) },
                { 2, new AnimatedSprite(SpaceBlasterGame.BackgroundMenu, new Vector2(0, 0), 1, 0.1, 480, 270, 4, 1f, new Vector2(0, 0)) }
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
    }
}
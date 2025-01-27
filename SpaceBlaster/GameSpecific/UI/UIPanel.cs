using SpaceBlaster;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Scene.Components;
using GameEngine.Graphics;
using GameEngine.Scene;

public class UIPanel : Position, IUIAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
    public UIPanel(float x, float y, Scene scene) : base(x, y, scene)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.TextureAtlas, new Vector2(140, 53), 1, 0.1, 32, 32, 13, 1f, new Vector2(0, 0)) },
            }
        );

        this.AnimationPlayer.SetCurrentAnimation(0);
    }
}
using System;
using System.Collections.Generic;
using GameEngine.Graphics;
using GameEngine.Scene;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using Microsoft.Xna.Framework;
using SpaceBlaster;

public class IntroStoryBook : Position, IArtificialIntelligence, IUIAnimatable
{
    public double TransitionTime = 5000;
    public double FadeTime = 750;
    public double CurrentTimer = 0;
    int currentSlide = 0;
    bool finished = false;
    Label storyText;
    string[] texts = new string[] {
        "You crash-landed on an unknown planet.",
        "The only structure here is a tower, \nstretching endlessly into the sky.",
        "It is populated by hostile \nrobots and humanoids.",
        "Defeat them all."
    };

    public AnimationPlayer AnimationPlayer { get; set; }

    public IntroStoryBook(float x, float y, Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.AnimationPlayer = new AnimationPlayer(
            new Dictionary<int, AnimatedSprite>(){
                { 0, new AnimatedSprite(SpaceBlasterGame.Intro1, new Vector2(0, 0), 1, 0.1, 128, 128, 4, 1f, new Vector2(0, 0)) },
                { 1, new AnimatedSprite(SpaceBlasterGame.Intro2, new Vector2(0, 0), 1, 0.1, 128, 128, 4, 1f, new Vector2(0, 0)) },
                { 2, new AnimatedSprite(SpaceBlasterGame.Intro3, new Vector2(0, 0), 1, 0.1, 128, 128, 4, 1f, new Vector2(0, 0)) },
                { 3, new AnimatedSprite(SpaceBlasterGame.Intro4, new Vector2(0, 0), 1, 0.1, 128, 128, 4, 1f, new Vector2(0, 0)) },
            }
        );

        this.storyText = new Label(0, 135 * 4, this.texts[0], Color.White, this.scene,this);
        this.scene.Spawn(this.storyText);

        this.AnimationPlayer.SetCurrentAnimation(0);
    }

    public void HandleAI(GameTime gameTime)
    {
        if (!this.finished)
        {
            this.CurrentTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.CurrentTimer > TransitionTime && this.currentSlide < 3)
            {
                this.AnimationPlayer.GetCurrentSprite().opacity = MathHelper.Clamp(MathHelper.Lerp(1f, 0f, MathHelper.Clamp((float)((this.CurrentTimer - this.TransitionTime) / (this.FadeTime)),0,1)), 0, 1);
            }
            else if (this.CurrentTimer <= FadeTime)
            {
                this.AnimationPlayer.GetCurrentSprite().opacity = MathHelper.Clamp(MathHelper.Lerp(0f, 1f, MathHelper.Clamp((float)((this.CurrentTimer) / (this.FadeTime)), 0, 1)), 0, 1);
            }

            if (this.CurrentTimer >= TransitionTime + FadeTime)
            {
                currentSlide++;

                if (currentSlide <= 3)
                {
                    this.storyText.Text = texts[currentSlide];
                    this.AnimationPlayer.SetCurrentAnimation(currentSlide);
                    this.AnimationPlayer.GetCurrentSprite().opacity = 0;
                    this.CurrentTimer = 0;
                }
                else if (currentSlide > 3)
                {
                    this.finished = true;
                }
            }
        }
    }
}

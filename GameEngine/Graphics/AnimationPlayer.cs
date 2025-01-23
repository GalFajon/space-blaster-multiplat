using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine.Graphics;

public class AnimationPlayer
{
    Dictionary<int, AnimatedSprite> Library = new Dictionary<int, AnimatedSprite>();
    public int CurrentAnimation = 0;

    public AnimationPlayer(Dictionary<int, AnimatedSprite> lib, int defaultAnimation = 0)
    {
        this.Library = lib;
        this.CurrentAnimation = defaultAnimation;

        if (this.Library.ContainsKey(CurrentAnimation)) this.Library[CurrentAnimation].Play();
    }

    public void SetCurrentAnimation(int c)
    {
        if (CurrentAnimation != c)
        {
            Library[CurrentAnimation].Pause();
            Library[CurrentAnimation].Reset();

            CurrentAnimation = c;
            Library[CurrentAnimation].Play();
        }
    }

    public void Update(GameTime gameTime)
    {
        Library[CurrentAnimation].Update(gameTime);
    }

    public AnimatedSprite GetCurrentSprite()
    {
        return Library[CurrentAnimation];
    }

}
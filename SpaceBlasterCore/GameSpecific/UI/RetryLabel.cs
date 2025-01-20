using GameSpecific;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster;

public class RetryLabel : Label, IArtificialIntelligence
{
    public RetryLabel(float x, float y, Color color, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, "", color, scene, parent)
    {
        this.Text = "";
    }

    public virtual void HandleAI(GameTime gameTime, SpaceBlaster.Scene.Scene level)
    {
        if (((Level)level)._player.Health == 0) this.Text = "Press R to retry.";
    }
}
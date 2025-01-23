using GameEngine.Scene;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using GameSpecific;
using Microsoft.Xna.Framework;

public class RetryLabel : Label, IArtificialIntelligence
{
    public RetryLabel(float x, float y, Color color, Scene scene, SceneObject parent = null) : base(x, y, "", color, scene, parent)
    {
        this.Text = "";
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        if (((Level)this.scene)._player.Health == 0) this.Text = "Press R to retry.";
    }
}
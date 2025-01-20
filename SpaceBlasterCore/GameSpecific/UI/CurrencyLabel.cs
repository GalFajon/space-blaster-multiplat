using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster;

public class CurrencyLabel : Label, IArtificialIntelligence
{
    public CurrencyLabel(float x, float y, Color color, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, "", color, scene, parent)
    {
        this.Text = "Currency: " + SpaceBlasterGame.Settings.Currency;
    }

    public virtual void HandleAI(GameTime gameTime, SpaceBlaster.Scene.Scene level)
    {
        this.Text = "Currency: " + SpaceBlasterGame.Settings.Currency;
    }
}
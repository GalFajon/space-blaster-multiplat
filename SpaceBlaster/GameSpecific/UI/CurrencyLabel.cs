using GameEngine.Scene;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using Microsoft.Xna.Framework;
using SpaceBlaster;

public class CurrencyLabel : Label, IArtificialIntelligence
{
    public CurrencyLabel(float x, float y, Color color, Scene scene, SceneObject parent = null) : base(x, y, "", color, scene, parent)
    {
        this.Text = "Currency: " + SpaceBlasterGame.Settings.Currency;
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        this.Text = "Currency: " + SpaceBlasterGame.Settings.Currency;
    }
}
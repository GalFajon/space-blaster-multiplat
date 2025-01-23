using GameEngine.Scene;
using GameEngine.Scene.Components;
using GameEngine.Scene.UI;
using GameSpecific;
using Microsoft.Xna.Framework;

public class WeaponLabel : Label, IArtificialIntelligence
{
    public WeaponLabel(float x, float y, Color color, Scene scene, SceneObject parent = null) : base(x, y, "", color, scene, parent)
    {
        this.Text = "";
    }

    public virtual void HandleAI(GameTime gameTime)
    {
        this.Text = "Weapon: " + ((Level)this.scene)._player.currentWeapon.stats.DisplayName;
    }
}
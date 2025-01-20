using GameSpecific;
using Microsoft.Xna.Framework;
using Scene.Components;
using SpaceBlaster;

public class WeaponLabel : Label, IArtificialIntelligence
{
    public WeaponLabel(float x, float y, Color color, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, "", color, scene, parent)
    {
        this.Text = "";
    }

    public virtual void HandleAI(GameTime gameTime, SpaceBlaster.Scene.Scene level)
    {
        this.Text = "Weapon: " + ((Level)level)._player.currentWeapon.stats.DisplayName + " [E]";
    }
}
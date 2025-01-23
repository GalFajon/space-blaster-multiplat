using Microsoft.Xna.Framework;
namespace GameEngine.Scene.Components;

public interface IArtificialIntelligence
{
    public virtual void HandleAI(GameTime gameTime) { return; }
}
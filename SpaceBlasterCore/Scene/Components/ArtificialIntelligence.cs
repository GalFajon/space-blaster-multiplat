using Microsoft.Xna.Framework;
using SpaceBlaster.Scene;
using GameSpecific;
namespace Scene.Components;

public interface IArtificialIntelligence
{
    public virtual void HandleAI(GameTime gameTime, SpaceBlaster.Scene.Scene level) { return; }
}
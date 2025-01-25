using Microsoft.Xna.Framework;
using GameSpecific;
using GameEngine.Graphics;

namespace GameEngine.Scene.Components;

public interface IUIAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
}
using GameEngine.Graphics;

namespace GameEngine.Scene.Components;

public interface IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
}
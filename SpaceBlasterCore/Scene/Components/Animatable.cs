using Microsoft.Xna.Framework;
using SpaceBlaster.Scene;
using GameSpecific;
namespace Scene.Components;

public interface IAnimatable
{
    public AnimationPlayer AnimationPlayer { get; set; }
}
using Microsoft.Xna.Framework;
using GameSpecific;
using GameEngine.Graphics;

namespace GameEngine.Scene.Components;

public interface IDrawable
{
    public Sprite sprite { get; set; }
}
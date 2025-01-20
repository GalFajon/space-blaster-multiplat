using Scene.Components;
using Microsoft.Xna.Framework;

using SpaceBlaster.Graphics;

public class Button : Position
{
    public Image image = null;
    public Label label = null;
    public Rectangle rect;

    public bool clicked = false;
    public bool Big = true;

    public Button(int x, int y, int width, int height, Image i, Label l, SpaceBlaster.Scene.Scene scene, SceneObject parent = null) : base(x, y, scene, parent)
    {
        this.rect = new Rectangle(0, 0, width, height);

        if (i != null)
        {
            image = i;
            i.Parent = this;
            scene.Spawn(image);
        }

        if (l != null)
        {
            label = l;
            l.Parent = this;
            scene.Spawn(label);
        }
    }

    public delegate void ClickHandler();
    public ClickHandler clickHandler = null;

    public void HandleClick() {
        if (clickHandler != null) this.clickHandler.Invoke();
    }
}
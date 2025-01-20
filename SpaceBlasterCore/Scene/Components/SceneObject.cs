using System.Collections;
using Scene;
namespace Scene.Components;

public abstract class SceneObject
{
    public SpaceBlaster.Scene.Scene scene;
    public bool ToDestroy { get; set; } = false;
    public void Destroy()
    {
        ToDestroy = true;
    }

    public SceneObject Parent { get; set; } = null;

    public SceneObject(SpaceBlaster.Scene.Scene scene, SceneObject parent = null)
    {
        this.scene = scene;
        this.Parent = parent;
    }
}
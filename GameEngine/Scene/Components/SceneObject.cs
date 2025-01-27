namespace GameEngine.Scene.Components;
public abstract class SceneObject
{
    public GameEngine.Scene.Scene scene;
    public bool ToDestroy { get; set; } = false;
    public void Destroy()
    {
        ToDestroy = true;
    }

    public SceneObject Parent { get; set; } = null;

    public SceneObject(GameEngine.Scene.Scene scene, SceneObject parent = null)
    {
        this.scene = scene;
        this.Parent = parent;
    }
}
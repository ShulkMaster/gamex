using System.Numerics;

namespace Gamex.Entity;

public abstract class SceneObject
{
  public Vector3 Location = Vector3.Zero;
  public Vector3 Rotation = Vector3.Zero;
  public Vector3 BoundingBox = Vector3.Zero;
  public float Scale = 1f;
  public string Name = "Object";
  public bool Enabled = true;

  protected abstract void RenderCustomControls();
  
  public void RenderControls()
  {
    RenderCustomControls();
  }
}
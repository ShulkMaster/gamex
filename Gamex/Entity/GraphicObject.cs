using Gamex.Mesh;
using Gamex.Program;

namespace Gamex.Entity;

public class GraphicObject: SceneObject
{
  private readonly ObjectMesh _mesh;
  private readonly GlProgram _program;

  public GraphicObject(ObjectMesh m, GlProgram p)
  {
    _mesh = m;
    _program = p;
  }
  
  protected override void RenderCustomControls()
  {
  }
}
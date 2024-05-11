using Gamex.Memory;
using Gamex.Program;
using OpenTK.Mathematics;

namespace Gamex.Mesh;

public class ObjectMesh
{
  public VertexBuffer Vbo { get; } = new();
  protected VertexArray _vao = new();
  public float Scale = 1f;
  protected Vector3 _location = new ();
  public GlProgram Program { get; private set; } = null!;

  public ObjectMesh(GlProgram p)
  {
    Program = p;
  }


}
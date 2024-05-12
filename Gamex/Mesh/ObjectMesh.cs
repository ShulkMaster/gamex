using Gamex.DataObjects;
using Gamex.Memory;

namespace Gamex.Mesh;

public sealed class ObjectMesh
{
  private readonly bool _normals;
  private readonly bool _textCoords;

  public ObjectMesh(bool n, bool t)
  {
    _normals = n;
    _textCoords = t;
  }

  public VertexBuffer Vbo { get; } = new();
  public VertexArray Vao = new();
  public List<MaterialProp> Materials = new();

  public bool HasNormal => _normals;
  public bool HasTextVCoors => _textCoords;
}

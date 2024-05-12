using Gamex.DataObjects;
using Gamex.Memory;

namespace Gamex.Mesh;

public sealed class ObjectMesh
{
  private bool _normals;
  private bool _textCoords;

  public VertexBuffer Vbo { get; } = new();
  public VertexArray Vao = new();
  public List<MaterialProp> Materials = new();

  public bool HasNormal => _normals;
  public bool HasTextVCoors => _textCoords;
}

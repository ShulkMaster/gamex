using Gamex.DataObjects;
using Gamex.Memory;
using ObjLoader.Loader.Data;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Loaders;

namespace Gamex.Model;

public class ObjectModel
{
  private VertexBuffer _vbo = new();
  private VertexArray vao;
  private ElementArrayBuffer _eao = new();
  private List<MaterialProp> _materials = new();

  public ObjectModel(LoadResult data)
  {
    vao = new VertexArray();
    FillVbo(data);
    FillGroups(data);
  }

  public IList<MaterialProp> Materials => _materials;

  private void FillVbo(LoadResult data)
  {
    const int perVertex = 3;
    VertexBufferLayout vbl = new();
    vbl.PushFloat(perVertex);
    //vbl.PushFloat(perNormal);

    // 1 vertex = 3 float
    int vertexCount = data.Vertices.Count;
    // 3 floats per vertex + 3 floats per normal
    int vertexSize = vertexCount * perVertex;
    // int normalsSize = (vertexCount + offset) * perNormal;
    float[] vertexData = new float[vertexSize];
    const int stride = perVertex;

    for (var i = 0; i < vertexCount; i++)
    {
      int index = i * stride;
      var vertex = data.Vertices[i];
      vertexData[index] = vertex.X;
      vertexData[index + 1] = vertex.Y;
      vertexData[index + 2] = vertex.Z;
    }

    // the VBO is currently bound
    _vbo.SetStaticData(vertexData);
    vao.AddBuffer(_vbo, vbl);
  }

  private void FillGroups(LoadResult data)
  {
    int totalFaces = 0;
    foreach (var group in data.Groups)
    {
      foreach (var face in group.Faces)
      {
        totalFaces += (face.Count - 2) * 3;
      }
    }

    var indices = new uint[totalFaces];

    int offset = 0;
    foreach (var group in data.Groups)
    {
      Console.WriteLine("Group {0} made of {1}", group.Name, group.Material?.Name ?? "Default");
      offset += FillMaterial(group, offset, indices);
    }

    _eao.SetStaticData(indices);
  }

  private MaterialProp SetMaterial(Material? mat)
  {
    var material = new MaterialProp();
    if (mat is null)
    {
      return material;
    }

    var ambient = mat.AmbientColor;
    var diffuse = mat.DiffuseColor;

    material.SetAmbient(ambient.X, ambient.Y, ambient.Z);
    material.SetDiffuse(diffuse.X, diffuse.Y, diffuse.Z);
    return material;
  }

  private int FillMaterial(Group group, int offset, IList<uint> indices)
  {
    var mat = SetMaterial(group.Material);
    var length = 0;
    foreach (var face in group.Faces)
    {
      var centralIndex = (uint)face[0].VertexIndex - 1;
      for (var index = 2; index < face.Count; index++)
      {
        var second = (uint)face[index - 1].VertexIndex;
        var third = (uint)face[index].VertexIndex;
        int position = offset + length + 3 * (index - 2);
        indices[position] = centralIndex;
        indices[position + 1] = second - 1;
        indices[position + 2] = third - 1;
      }
      length += (face.Count - 2) * 3;
    }

    mat.Range = new MaterialRange { Offset = offset, Count = length };
    if (length > 0)
    {
      _materials.Add(mat);
    }
    return length;
  }
}
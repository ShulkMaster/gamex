using Gamex.Memory;
using Gamex.Mesh;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;

namespace Gamex.Loader;

public static class MeshLoader
{
  const int PerVertex = 3;
  const int PerNormal = 3;
  const int PerTexture = 2;

  public static ObjectMesh LoadMesh(LoadResult data)
  {
    var vertexCount = (uint)data.Vertices.Count;
    bool hasNormals = data.Normals.Any();
    bool hasTextures = data.Textures.Any();
    float[] buffer = AllocateVertex(vertexCount, hasNormals, hasTextures);
    var layout = FillLayout(hasNormals, hasTextures);

    int floatStride = layout.Stride / sizeof(float);
    FillVertex(buffer, data.Vertices, floatStride);
    FillNormal(buffer, data.Normals, floatStride);
    ObjectMesh mesh = new(hasNormals, hasTextures);
    mesh.Vbo.SetStaticData(buffer);
    mesh.Vao.AddBuffer(layout);
    return mesh;
  }

  private static VertexBufferLayout FillLayout(bool hasNormals, bool hasTextures)
  {
    VertexBufferLayout vbl = new();
    vbl.PushFloat(PerVertex);

    if (hasNormals)
    {
      vbl.PushFloat(PerNormal);
    }

    if (hasTextures)
    {
      vbl.PushFloat(PerTexture);
    }

    return vbl;
  }

  private static float[] AllocateVertex(uint vertex, bool normals, bool textures)
  {
    uint vCount = vertex * PerVertex;
    uint nCount = normals ? vertex * PerNormal : 0;
    uint tCount = textures ? vertex * PerTexture : 0;
    return new float[vCount + nCount + tCount];
  }

  private static void FillVertex(IList<float> buff, IList<Vertex> vertex, int stride)
  {
    for (var i = 0; i < vertex.Count; i++)
    {
      int index = i * stride;
      var element = vertex[i];
      buff[index] = element.X;
      buff[index + 1] = element.Y;
      buff[index + 2] = element.Z;
    }
  }

  private static void FillNormal(IList<float> buff, IList<Normal> normals, int stride)
  {
    const int normalOffset = 3;
    for (var i = 0; i < normals.Count; i++)
    {
      int index = i * stride + normalOffset;
      var element = normals[i];
      buff[index] = element.X;
      buff[index + 1] = element.Y;
      buff[index + 2] = element.Z;
    }
  }
}
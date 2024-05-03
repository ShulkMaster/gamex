using Gamex.Memory;
using ObjLoader.Loader.Loaders;

namespace Gamex.Model;

public class ObjectModel
{
  private VertexBuffer vbo;

  public ObjectModel(LoadResult data)
  {
    vbo = new VertexBuffer();
    FillVbo(data);
  }

  private void FillVbo(LoadResult data)
  {
    Console.WriteLine("The vertex count {0}", data.Vertices.Count);
    Console.WriteLine("The Normals count {0}", data.Normals.Count);

    /* OBJ indices are 1 index based, not 0, so we fill with 0s the firsts position,
     * the extra position is never used, so we saved on recalculating all the indices by -1
     */
    const int offset = 1;
    const int perVertex = 3;
    const int perNormal = 3;
    // 1 vertex = 3 float
    int vertexCount = data.Vertices.Count;
    // 3 floats per vertex + 3 floats per normal
    int vertexSize = (vertexCount + offset) * perVertex;
    int normalsSize = (vertexCount + offset) * perNormal;
    float[] vertexData = new float[vertexSize + normalsSize];
    const int stride = perVertex + perNormal;
   

    for (int i = 1; i < vertexCount; i++)
    {
      int index = i * stride;
      var vertex = data.Vertices[i - 1];
      vertexData[index] = vertex.X;
      vertexData[index + 1] = vertex.Y;
      vertexData[index + 2] = vertex.Z;
    }

    vbo.SetStaticData(vertexData);
  }
}
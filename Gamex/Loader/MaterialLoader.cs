using Gamex.DataObjects;
using ObjLoader.Loader.Data;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Loaders;

namespace Gamex.Loader;

public static class MaterialLoader
{
  private static uint[] AllocateIndex(LoadResult data)
  {
    var totalFaces = 0;
    foreach (var group in data.Groups)
    {
      foreach (var face in group.Faces)
      {
        totalFaces += (face.Count - 2) * 3;
      }
    }

    return new uint[totalFaces];
  }
 
  private static int FillMaterial(Group group, int offset, IList<uint> indices)
  {
    var length = 0;
    foreach (var face in group.Faces)
    {
      uint centralIndex = (uint)face[0].VertexIndex - 1;
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

    return length;
  }
  
  private static MaterialProp SetMaterial(Material? mat)
  {
    var material = new MaterialProp();
    if (mat is null)
    {
      return material;
    }

    var ambient = mat.AmbientColor;
    var diffuse = mat.DiffuseColor;

    material.Ambient = LinearMath.ToTkVector3(ambient);
    material.Diffuse = LinearMath.ToTkVector3(diffuse);
    return material;
  }

  public static List<MaterialProp> LoadMaterials(LoadResult data)
  {
    var materials = new List<MaterialProp>();
    uint[] indices = AllocateIndex(data);
    var offset = 0;
    foreach (var group in data.Groups)
    {
      int lenght = FillMaterial(group, offset, indices);
      if (lenght > 0)
      {
        var material = SetMaterial(group.Material);
        material.Range = new MaterialRange { Offset = offset, Count = lenght };
        materials.Add(material);
      }
      offset += lenght;
    }

    return materials;
  }
}
using OpenTK.Mathematics;

namespace Gamex.DataObjects;

public struct MaterialRange
{
  public int Count;
  public int Offset;
}

public sealed class MaterialProp
{
  public string Name  = "";
  public float Shyne = 0.5f;
  public MaterialRange Range = new();
  public Vector3 Ambient = new(.1f);
  public Vector3 Diffuse = Vector3.One;
  public Vector3 Specular = Vector3.Zero;

  private string VectorToColor(Vector3 v)
  {
    return $"r{v.X} g{v.Y} b{v.Z}";
  }

  public string Print()
  {
    return $"{Name}: Diffuse Kd: ({VectorToColor(Diffuse)}) \n" +
           $"{Name}: Ambient Ka: ({VectorToColor(Ambient)})";
  }
}
using OpenTK.Mathematics;

namespace Gamex.DataObjects;

public struct MaterialRange
{
  public int Count;
  public int Offset;
}

public class MaterialProp
{
  public MaterialRange Range = new(){ Count = 0, Offset = 0 };

  public string Name { get; set; } = "";

  public Vector3 Ambient { get; set; } = new(.1f);
  public Vector3 Diffuse { get; set; } = new(1f);

  public void SetAmbient(float r, float g, float b)
  {
    Ambient = new Vector3(r, g, b);
  }
  
  public void SetDiffuse(float r, float g, float b)
  {
    Diffuse = new Vector3(r, g, b);
  }

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
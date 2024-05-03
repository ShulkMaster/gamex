namespace Gamex.DataObjects;

public class Material
{
  private float[] ambient = { .1f, .1f, .1f };
  private float[] diffuse = { 1f, 1f, 1f };
  // private float[] Specular;
  // private float Shininess;

  public string Name { get; set; } = "";

  public void SetAmbient(float r, float g, float b)
  {
    ambient[0] = r;
    ambient[1] = g;
    ambient[2] = b;
  }
  
  public void SetDiffuse(float r, float g, float b)
  {
    diffuse[0] = r;
    diffuse[1] = g;
    diffuse[2] = b;
  }

}
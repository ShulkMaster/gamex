using Gamex.Program;

namespace Gamex.DataObjects;

public class UniformLocation
{
  public int MaterialAmbientLoc { get; private set; }
  public int MaterialDiffuse { get; private set; }
  public int LightColor { get; private set; }
  public int LightLocation { get; private set; }

  public void Config(GlProgram prog)
  {
    MaterialAmbientLoc = prog.FindUniform("material.ambient");
    MaterialDiffuse = prog.FindUniform("material.diffuse");
    LightLocation = prog.FindUniform("light.loc");
    LightColor = prog.FindUniform("light.color");
  }
}
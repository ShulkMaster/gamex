using Gamex.DataObjects;
using Gamex.Mesh;
using Gamex.Model;
using Gamex.Program;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Gamex.Entity;

public class GraphicObject: SceneObject
{
  private readonly ObjectMesh _mesh;
  public static GlProgram Program;
  private static int _uniMatProj;
  private static int _uniMatView;
  private static int _uniMatModel;
  private static int _uniMatInvertModel;
  private static int _uniMaterialAm;
  private static int _uniMaterialDiff;
  private static int _uniLLoc;
  private static int _uniLcolor;

  public GraphicObject(ObjectMesh m)
  {
    _mesh = m;
  }

  public static bool Initialize()
  {
    var vs = new VertexShader();
    var fs = new FragmentShader();
    bool compiled = vs.Compile("model") && fs.Compile("model");
    if (!compiled)
    {
      Console.Error.WriteLine(vs.InfoLog);
      Console.Error.WriteLine(fs.InfoLog);
      vs.Dispose();
      fs.Dispose();
      return false;
    }
    
    Program = new GlProgramBuilder()
      .AttachVertex(vs)
      .AttachFragment(fs)
      .Build();
    
    _uniMatProj = Program.FindUniform("projection");
    _uniMatView = Program.FindUniform("view");
    _uniMatModel = Program.FindUniform("model");
    _uniMatInvertModel = Program.FindUniform("invertModel");
    _uniMaterialAm = Program.FindUniform("material.ambient");
    _uniMaterialDiff = Program.FindUniform("material.diffuse");
    _uniLLoc = Program.FindUniform("light.loc");
    _uniLcolor = Program.FindUniform("light.color");

    vs.Dispose();
    fs.Dispose();
    return true;
  }

  protected override void RenderCustomControls()
  {
  }

  public void Render(Matrix4 view, Matrix4 proj, PointLight l)
  {
    var model = Matrix4.Identity;
    model *= Matrix4.CreateScale(Scale);
    model *= Matrix4.CreateTranslation(Location.X, Location.Y, Location.Z);
    GL.UniformMatrix4(_uniMatProj, false, ref proj);
    GL.UniformMatrix4(_uniMatView, false, ref view);
    GL.UniformMatrix4(_uniMatModel, false, ref model);
    var inverse = Matrix4.Invert(model);
    inverse.Transpose();
    var normalInvert = new Matrix3(inverse);
    GL.UniformMatrix3(_uniMatInvertModel, false, ref normalInvert);
    GL.Uniform3(_uniLLoc, LinearMath.ToTkVector3(l.Location));
    GL.Uniform3(_uniLcolor, LinearMath.ToTkVector3(l.Color));
    _mesh.Vao.Bind();
    foreach (var material in _mesh.Materials)
    {
      GL.Uniform3(_uniMaterialAm, material.Ambient);
      GL.Uniform3(_uniMaterialDiff, material.Diffuse);
      var r = material.Range;
      int pointer = r.Offset * sizeof(uint);
      GL.DrawElements(PrimitiveType.Triangles, r.Count, DrawElementsType.UnsignedInt, pointer);
    }
  }
}
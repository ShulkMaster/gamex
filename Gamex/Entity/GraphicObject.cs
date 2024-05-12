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
  private static int _projMatUniform;
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
    
    _projMatUniform = Program.FindUniform("projection");
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
    var mat = Matrix4.CreateScale(Scale);
    mat = Matrix4.CreateTranslation(LinearMath.ToTkVector3(Location)) * mat;
    mat = mat * view * proj;
    GL.UniformMatrix4(_projMatUniform, false, ref mat);
    GL.Uniform3(_uniLLoc, LinearMath.ToTkVector3(l.Location));
    GL.Uniform3(_uniLcolor, LinearMath.ToTkVector3(l.Color));
    _mesh.Vao.Bind();
    foreach (var material in _mesh.Materials)
    {
      GL.Uniform3(_uniMaterialAm, material.Ambient);
      GL.Uniform3(_uniMaterialDiff, material.Diffuse);
      var r = material.Range;
      GL.DrawArrays(PrimitiveType.Triangles, r.Offset, r.Count);
    }
  }
}
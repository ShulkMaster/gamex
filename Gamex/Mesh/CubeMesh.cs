using Gamex.Memory;
using Gamex.Program;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Gamex.Mesh;

public class CubeMesh
{
  private VertexBuffer _vbo = new();
  private VertexArray _vao = new();
  public static GlProgram Program = null!;
  private static int _projMatUniform;
  private static int _colorUniform;
  public Vector3 Loc = new ();
  public float Scale = 1f;

  public CubeMesh()
  {
    const int perVertex = 3;
    const int perNormal = 3;
    VertexBufferLayout vbl = new();
    vbl.PushFloat(perVertex);
    vbl.PushFloat(perNormal);
    _vbo.SetStaticData(_vertices);
    _vao.AddBuffer(vbl);
  }

  public static bool Initialize()
  {
    var vs = new VertexShader();
    var fs = new FragmentShader();
    bool compiled = vs.Compile("cube") && fs.Compile("cube");
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

    _projMatUniform = Program.FindUniform("projectionM");
    _colorUniform = Program.FindUniform("color");
    vs.Dispose();
    fs.Dispose();
    return true;
  }

  public void Render(Matrix4 proj, Matrix4 view, Vector3 color)
  {
    var mat = Matrix4.CreateScale(Scale);
    mat = Matrix4.CreateTranslation(Loc) * mat;
    mat = mat * view * proj;
    GL.UniformMatrix4(_projMatUniform, false, ref mat);
    GL.Uniform3(_colorUniform, color);
    _vao.Bind();
    GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);
  }
  
  private void ConfigLight()
  {
    // GL.Uniform3(_location.LightLocation, LinearMath.ToTkVector3(l.Location));
    // GL.Uniform3(_location.LightColor, LinearMath.ToTkVector3(l.Color));
  }

  public static void Clear()
  {
    Program.Dispose();
  }

  private readonly float[] _vertices = {
    -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
    0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 
    0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 
    0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 
    -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 
    -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 

    -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
    0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
    0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
    0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,

    -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
    -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
    -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
    -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
    -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
    -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

    0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
    0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
    0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
    0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
    0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
    0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
    0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
    0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
    0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

    -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
    0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
    0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
    0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
  };
}
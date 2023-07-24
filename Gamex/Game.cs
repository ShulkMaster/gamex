using Gamex.DataObjects;
using Gamex.Loader;
using Gamex.Program;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Gamex;

public class Game : GameWindow
{
    private GlProgram? _program;
    private int _vertexArrayObject;
    private int _elementBufferObject;
    private int faceCount;
    
    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings { Size = (width, height), Title = title })
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        var loader = new AssetLoader("Fox");
        var stuff = loader.Load();
        var vShader = new VertexShader();
        if (!vShader.Compile("position.vert"))
        {
            Console.Error.WriteLine(vShader.InfoLog);
            return;
        }

        var fShader = new FragmentShader();
        if (!fShader.Compile("color.frag"))
        {
            Console.Error.WriteLine(fShader.InfoLog);
            return;
        }
        
        _program = new GlProgramBuilder()
            .AttachVertex(vShader)
            .AttachFragment(fShader)
            .Build();
        
        var vbo = new VertexBuffer();
        VertexPack pack = loader.Pack(stuff);
        vbo.SetStaticData(pack.Vertices);
        
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, pack.Size, pack.PointerType, false, pack.stride, 0);
        GL.EnableVertexAttribArray(0);
        
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

        uint[] indices = pack.Indices;
        faceCount = indices.Length;
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        _program.UseProgram();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        _program?.UseProgram();
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, faceCount, DrawElementsType.UnsignedInt, 0);
        SwapBuffers();
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _program?.Dispose();
    }
}
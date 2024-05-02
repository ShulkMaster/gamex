using Gamex.DataObjects;
using Gamex.Loader;
using Gamex.Program;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Gamex;

public class Game : GameWindow
{
    private GlProgram? _program;
    private int _vertexArrayObject;
    private int _elementBufferObject;
    private int faceCount;
    private int _rotationMatrixLocation;

    private float _rotationX = 0;
    private float _rotationY = 0;
    private float _rotationZ = 0;
    private float _scale = 1;

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

        // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

        uint[] indices = pack.Indices;
        faceCount = indices.Length;
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices,
            BufferUsageHint.StaticDraw);
        _program.UseProgram();
        _rotationMatrixLocation = _program.FindUniform("rotationMatrix");
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        const float clamp = 6.28f;
        float temp = _scale + (float)(MouseState.ScrollDelta.Y * 0.01);
        _scale = Math.Clamp(temp, 0.05f, 2f);

        if (KeyboardState.IsKeyDown(Keys.LeftShift))
        {
            _rotationZ -= 0.0174533f / 10 * MouseState.Delta.X;
        }


        _rotationX = Math.Clamp(temp, -clamp, clamp);

        if (MouseState.IsButtonDown(MouseButton.Left))
        {
            _rotationY -= 0.0174533f / 10 * MouseState.Delta.X;
            _rotationY = Math.Clamp(_rotationY, -clamp, clamp);

            _rotationX -= 0.0174533f / 10 * MouseState.Delta.Y;
            _rotationX = Math.Clamp(_rotationX, -clamp, clamp);
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        _program?.UseProgram();
        Matrix4 rotationMatrix = Matrix4.Identity * Matrix4.CreateRotationX(_rotationX);
        rotationMatrix *= Matrix4.CreateRotationY(_rotationY);
        rotationMatrix *= Matrix4.CreateRotationZ(_rotationZ);
        rotationMatrix *= Matrix4.CreateScale(_scale);

        GL.UniformMatrix4(_rotationMatrixLocation, false, ref rotationMatrix);
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
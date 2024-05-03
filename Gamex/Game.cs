using Gamex.DataObjects;
using Gamex.Loader;
using Gamex.Model;
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
    private int _rotationMatrixLocation;
    private int _matUniform;

    private float _rotationX;
    private float _rotationY;
    private float _rotationZ;
    private float _scale = 1;

    private ObjectModel? _model;

    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings { Size = (width, height), Title = title })
    {
    }

    protected override void OnLoad()
    {
        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(DepthFunction.Less);
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        var loader = new AssetLoader("Shiba");
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
        _model = new ObjectModel(stuff);
        // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        _program.UseProgram();
        _rotationMatrixLocation = _program.FindUniform("rotationMatrix");
        _matUniform = _program.FindUniform("material");
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

        if (MouseState.IsButtonDown(MouseButton.Left))
        {
            _rotationY -= 0.0174533f / 10 * MouseState.Delta.X;
            _rotationY = Math.Clamp(_rotationY, -clamp, clamp);

            _rotationX -= 0.0174533f / 10 * MouseState.Delta.Y;
            _rotationX = Math.Clamp(_rotationX, -clamp, clamp);
        }
    }

    private void ConfigMaterial(MaterialProp mat)
    {
        GL.Uniform3(_matUniform, mat.Ambient);
        GL.Uniform3(_matUniform + 1, mat.Diffuse);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        Matrix4 rotationMatrix = Matrix4.Identity * Matrix4.CreateRotationX(_rotationX);
        rotationMatrix *= Matrix4.CreateRotationY(_rotationY);
        rotationMatrix *= Matrix4.CreateRotationZ(_rotationZ);
        rotationMatrix *= Matrix4.CreateScale(_scale);

        GL.UniformMatrix4(_rotationMatrixLocation, false, ref rotationMatrix);
        var materials = _model!.Materials;
        foreach (var material in materials)
        {
            var range = material.Range;
            int pointer = range.Offset * sizeof(uint);
            ConfigMaterial(material);
            GL.DrawElements(PrimitiveType.Triangles, range.Count, DrawElementsType.UnsignedInt, pointer);
        }
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
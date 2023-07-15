using Gamex.Program;
using OpenTK.Graphics.ES20;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Gamex;

public class Game : GameWindow
{
    
    private readonly float[] vertices = {
        -0.5f, -0.5f, 0.0f, //Bottom-left vertex
        0.5f, -0.5f, 0.0f, //Bottom-right vertex
        0.0f,  0.5f, 0.0f  //Top vertex
    };

    private VertexShader _vShader = new();
    private FragmentShader _fShader = new();

    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings { Size = (width, height), Title = title })
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        if (!_vShader.Compile("position.vert"))
        {
            Console.Error.WriteLine(_vShader.InfoLog);
            return;
        }

        if (!_fShader.Compile("color.frag"))
        {
            Console.Error.WriteLine(_fShader.InfoLog);
        }
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
        _vShader.Dispose();
        _fShader.Dispose();
    }
}
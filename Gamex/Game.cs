using System.Numerics;
using Gamex.DataObjects;
using Gamex.Loader;
using Gamex.Model;
using Gamex.Program;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using Vector3 = System.Numerics.Vector3;

namespace Gamex;

public class Game : GameWindow
{
    private ImGuiController _controller;
    private GlProgram? _program;
    private int _rotationMatrixLocation;
    private int _matUniform;

    private Vector3 _translation = new (0f);
    private Vector3 _rotation = new (0f);
    private Matrix4 _projectionMatrix = Matrix4.Identity;
    private float _scale = 1;

    private ObjectModel? _model;
    private LightPanel _lPanel = new();

    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings { Size = (width, height), Title = title, APIVersion = new Version(3, 3), Vsync = VSyncMode.On})
    {
    }

    protected override void OnLoad()
    {
        _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
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
        var translation = Matrix4.CreateTranslation(new OpenTK.Mathematics.Vector3(
            _translation.X,
            _translation.Y,
            _translation.Z
        ));

        var rotation = Matrix4.CreateRotationX(_rotation.X) 
                       * Matrix4.CreateRotationY(_rotation.Y)
                       * Matrix4.CreateRotationZ(_rotation.Z);
        _projectionMatrix = translation * rotation * Matrix4.CreateScale(_scale);
    }

    private void ConfigMaterial(MaterialProp mat)
    {
        GL.Uniform3(_matUniform, mat.Ambient);
        GL.Uniform3(_matUniform + 1, mat.Diffuse);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _controller.Update(this, (float)args.Time);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        ImGui.Begin("Model position");
        float fps = ImGui.GetIO().Framerate;
        ImGui.Text($"FPS {fps}");
        ImGui.SliderFloat("Scale", ref _scale, 0.05f, 3f);
        ImGui.SliderFloat3("Translation", ref _translation, -1f, 1f);
        ImGui.SliderFloat3("Rotation", ref _rotation, -3.1415f, 3.1415f);
        ImGui.End();

        GL.UniformMatrix4(_rotationMatrixLocation, false, ref _projectionMatrix);
        var materials = _model!.Materials;
        foreach (var material in materials)
        {
            var range = material.Range;
            int pointer = range.Offset * sizeof(uint);
            ConfigMaterial(material);
            GL.DrawElements(PrimitiveType.Triangles, range.Count, DrawElementsType.UnsignedInt, pointer);
        }
        _lPanel.Render();
        _controller.Render();
        ImGuiController.CheckGLError("End of frame");
        SwapBuffers();
    }
    
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        _controller.MouseScroll(e.Offset);
    }
    
    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        _controller.PressChar((char)e.Unicode);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        _controller.WindowResized(ClientSize.X, ClientSize.Y);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _program?.Dispose();
        _controller.Dispose();
    }
}
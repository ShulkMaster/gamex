using Gamex.DataObjects;
using Gamex.Mesh;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using V3 = System.Numerics.Vector3;

namespace Gamex;

public class Game : GameWindow
{
    private ImGuiController _controller;
    private Vector3 _rotation = new (0f);
    private readonly Matrix4 _projectionMatrix = Matrix4.CreatePerspectiveOffCenter(-1f, 1f, -1f, 1f, 1f, 9f);
    private Vector3 _camLoc = new (0f, 0f, 2f); 
    private Vector3 _camTarget = new (0f, 0f, -1f); 
    private readonly LightPanel _lPanel = new();

    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings { Size = (width, height), Title = title, APIVersion = new Version(3, 3), Vsync = VSyncMode.On})
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(DepthFunction.Less);
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        CubeMesh.Initialize();
        _lPanel.AddLight(V3.Zero);
        _lPanel.AddLight(V3.Zero);
        _lPanel.AddLight(V3.Zero);
        _lPanel.Lights[0].Location = new System.Numerics.Vector3(0.5f, 0.5f, 0.5f);
        // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
    }

    private void ConfigMaterial(MaterialProp mat)
    {
        // GL.Uniform3(_location.MaterialAmbientLoc, mat.Ambient);
        // GL.Uniform3(_location.MaterialDiffuse, mat.Diffuse);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        var walkSpeed = (float)(4d * RenderTime);
        base.OnKeyDown(e);
        switch (e.Key)
        {
            case Keys.W:
                _camLoc += walkSpeed * _camTarget;
                break;
            case Keys.S:
                _camLoc -= walkSpeed * _camTarget;
                break;
            case Keys.D:
                _camLoc += Vector3.Cross(_camTarget, Vector3.UnitY).Normalized() * walkSpeed;
                break;
            case Keys.A:
                _camLoc -= Vector3.Cross(_camTarget, Vector3.UnitY).Normalized() * walkSpeed;
                break;
            case Keys.E:
                _camLoc -= walkSpeed * Vector3.UnitY;
                break;
            case Keys.Q:
                _camLoc += walkSpeed * Vector3.UnitY;
                break;
        }
        
        float rotationSpeed = 0.05f;
        switch (e.Key)
        {
            case Keys.Up:
                _rotation.Y = Math.Clamp(rotationSpeed + _rotation.Y, -3.14f, 3.14f);
                break;
            case Keys.Down:
                _rotation.Y = Math.Clamp(_rotation.Y - rotationSpeed, -3.14f, 3.14f);
                break;
            case Keys.Left:
                _rotation.X = Math.Clamp(_rotation.X + rotationSpeed, -3.14f, 3.14f);
                break;
            case Keys.Right:
                _rotation.X = Math.Clamp(_rotation.X - rotationSpeed, -3.14f, 3.14f);
                break;
        }

        var direction = new Vector3
        {
            X = (float)(Math.Cos(_rotation.X) * Math.Cos(_rotation.Y)),
            Z = (float)(Math.Sin(_rotation.X) * Math.Cos(_rotation.Y)),
            Y = (float)Math.Sin(_rotation.Y)
        };
        _camTarget = direction.Normalized();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        var view = Matrix4.LookAt(_camLoc, _camLoc + _camTarget, Vector3.UnitY);
        _controller.Update(this, (float)args.Time);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        // var mat = Matrix4.CreateRotationY(_rotation.Y) * Matrix4.CreateRotationX(_rotation.X); 
        _lPanel.Render(view, _projectionMatrix);

        ImGui.Begin("Info");
        float fps = ImGui.GetIO().Framerate;
        ImGui.Text($"FPS {fps}");
        ImGui.End();

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
        _controller.Dispose();
        CubeMesh.Clear();
    }
}
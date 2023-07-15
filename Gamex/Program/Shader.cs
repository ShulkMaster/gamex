using OpenTK.Graphics.OpenGL;

namespace Gamex.Program;

public abstract class Shader: IDisposable
{
    private const string ShaderDir = "shaders";
    private readonly ShaderType _type;
    private int _handle;
    private bool _disposed;

    public string InfoLog { get; private set; } = string.Empty;

    protected Shader(ShaderType type)
    {
        _type = type;
    }
    
    ~Shader()
    {
        if (!_disposed)
        {
            Console.Error.WriteLine("GPU Shader leak! Did you forget to call Dispose?");
        }
        Dispose(false);
    }

    private string GetShaderPath(string fileName)
    {
        string shaderKind = _type switch
        {
            ShaderType.VertexShader => "vertex",
            ShaderType.FragmentShader => "fragment",
            _ => throw new ArgumentOutOfRangeException(nameof(fileName), "Unsupported shader type")
        };

        return Path.Combine(ShaderDir, shaderKind, fileName);
    }

    public bool Compile(string filaName)
    {
        string path = GetShaderPath(filaName);
        string source = File.ReadAllText(path);
        _handle = GL.CreateShader(_type);
        GL.ShaderSource(_handle, source);
        GL.CompileShader(_handle);
        GL.GetShader(_handle, ShaderParameter.CompileStatus, out int status);
        GL.GetShaderInfoLog(_handle, out string infoLog);
        InfoLog = infoLog;
        return status == 1;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        GL.DeleteShader(_handle);
        _disposed = disposing;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
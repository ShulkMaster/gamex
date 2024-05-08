using OpenTK.Graphics.OpenGL;

namespace Gamex.Program;

public abstract class Shader: IDisposable
{
    private const string ShaderDir = "shaders";
    private readonly ShaderType _type;
    private readonly int _handle;
    private bool _disposed;

    public string InfoLog { get; private set; } = string.Empty;

    protected Shader(ShaderType type)
    {
        _type = type;
        _handle = GL.CreateShader(_type);
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
        string ext = _type switch
        {
            ShaderType.VertexShader => "vert",
            ShaderType.FragmentShader => "frag",
            _ => throw new ArgumentOutOfRangeException(nameof(fileName), "Unsupported shader type")
        };

        return Path.Combine(ShaderDir, fileName, $"{fileName}.{ext}");
    }
    
    public int GetHandle() => _handle;

    public bool Compile(string filaName)
    {
        string path = GetShaderPath(filaName);
        string source = File.ReadAllText(path);
        GL.ShaderSource(_handle, source);
        GL.CompileShader(_handle);
        GL.GetShader(_handle, ShaderParameter.CompileStatus, out int status);
        GL.GetShaderInfoLog(_handle, out string infoLog);
        InfoLog = infoLog;
        return status == 1;
    }

    private void Dispose(bool disposing)
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
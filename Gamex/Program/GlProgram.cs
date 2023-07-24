using OpenTK.Graphics.OpenGL;

namespace Gamex.Program;

public sealed class GlProgram: IDisposable
{
    private readonly int _handle;
    private bool _disposed;

    public GlProgram(int handle)
    {
        _handle = handle;
    }
    
    ~GlProgram()
    {
        if (!_disposed)
        {
            Console.Error.WriteLine("GPU Program leak! Did you forget to call Dispose?");
        }
    }

    public void UseProgram()
    {
        GL.UseProgram(_handle);
    }

    public void Dispose()
    {
        if (_disposed) return;
        GL.DeleteProgram(_handle);
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public int FindUniform(string name)
    {
        return GL.GetUniformLocation(_handle, name);
    }
}
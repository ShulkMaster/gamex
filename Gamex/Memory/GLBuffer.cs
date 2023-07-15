using OpenTK.Graphics.OpenGL;

namespace Gamex.Memory;

public abstract class GlBuffer
{
    private readonly int _handle;
    private readonly BufferTarget _target;

    protected GlBuffer(BufferTarget target)
    {
        _handle = GL.GenBuffer();
        _target = target;
    }
    
    ~GlBuffer()
    {
        GL.DeleteBuffer(_handle);
    }

    protected void Bind()
    {
        GL.BindBuffer(_target, _handle);
    }
    
    protected void Unbind()
    {
        GL.BindBuffer(_target, 0);
    }

    protected void SetData<TData>(int size, TData[] data, BufferUsageHint hint) where TData : struct
    {
        GL.BufferData(_target, size, data, hint);
    }
}
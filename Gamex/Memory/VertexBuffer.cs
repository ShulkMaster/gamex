using OpenTK.Graphics.OpenGL;

namespace Gamex.Memory;

public class VertexBuffer: GlBuffer
{
    public VertexBuffer(): base(BufferTarget.ArrayBuffer)
    {
    }
    
    public void SetStaticData(float[] data)
    {
        int size = data.Length * sizeof(float);
        Bind();
        SetData(size, data, BufferUsageHint.StaticDraw);
    }
}
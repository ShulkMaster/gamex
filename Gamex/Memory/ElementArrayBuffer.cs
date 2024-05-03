using OpenTK.Graphics.OpenGL;

namespace Gamex.Memory;

public class ElementArrayBuffer: GlBuffer
{
  public ElementArrayBuffer() : base(BufferTarget.ElementArrayBuffer)
  {
  }
  
  public void SetStaticData(uint[] data)
  {
    int size = data.Length * sizeof(uint);
    Bind();
    SetData(size, data, BufferUsageHint.StaticDraw);
  }
}
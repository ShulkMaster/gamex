using OpenTK.Graphics.OpenGL;

namespace Gamex.Memory;

public class VertexArray
{
  private int handle;

  public VertexArray()
  {
    handle = GL.GenVertexArray();
  }

  public void Bind()
  {
    GL.BindVertexArray(handle);
  }

  public void AddBuffer(VertexBufferLayout layout)
  {
    var elements = layout.Elements;
    var offset = 0;
    
    Bind();
    for (var index = 0; index < elements.Count; index++)
    {
      var element = elements[index];
      GL.EnableVertexAttribArray(index);
      GL.VertexAttribPointer(index, element.Count, element.Type, element.Normalized, layout.Stride, offset);
      offset += element.Count * VertexBufferLayout.GetElementSize(element.Type);
    }
  }
}
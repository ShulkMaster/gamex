using OpenTK.Graphics.OpenGL;

namespace Gamex.Memory;

public struct VertexBufferElement {
  public VertexAttribPointerType Type;
  public int Count;
  public bool Normalized;
}

public class VertexBufferLayout
{
  private List<VertexBufferElement> _list = new();
  
  public IList<VertexBufferElement> Elements => _list;

  public int Stride { get; private set; } = 0;
  
  private void Push(VertexBufferElement element, int tSize)
  {
    _list.Add(element);
    Stride += tSize * element.Count;
  }

  public void PushFloat(int count, bool normalized = false)
  {
    VertexBufferElement element = new()
    {
      Type = VertexAttribPointerType.Float,
      Count = count,
      Normalized = normalized
    };
    Push(element, sizeof(float));
  }

  public static int GetElementSize(VertexAttribPointerType type)
  {
    return type switch
    {
      VertexAttribPointerType.Float => sizeof(float),
      VertexAttribPointerType.Int => sizeof(int),
      VertexAttribPointerType.Byte => sizeof(byte),
      VertexAttribPointerType.UnsignedInt => sizeof(uint),
      _ => throw new NotImplementedException("The Attrib type is not defined")
    };
  }

  public static VertexAttribPointerType GetElementType<T>() where T : struct
  {
    var t = typeof(T);

    if (t == typeof(float)) return VertexAttribPointerType.Float;
    if (t == typeof(int)) return VertexAttribPointerType.Int;
    if (t == typeof(byte)) return VertexAttribPointerType.Byte;
    if (t == typeof(uint)) return VertexAttribPointerType.UnsignedInt;
    
    throw new NotImplementedException("Type T type is not defined");
  }
}
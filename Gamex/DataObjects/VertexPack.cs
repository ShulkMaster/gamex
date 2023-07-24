using OpenTK.Graphics.OpenGL;

namespace Gamex.DataObjects;

public struct VertexPack
{
    public readonly float[] Vertices;
    public readonly uint[] Indices;
    public int stride = 3 * sizeof(float);
    public int offset = 0;
    public int Size = 3;
    public VertexAttribPointerType PointerType = VertexAttribPointerType.Float;

    public VertexPack(float[] v, uint[] i)
    {
        Vertices = v;
        Indices = i;
    }
}
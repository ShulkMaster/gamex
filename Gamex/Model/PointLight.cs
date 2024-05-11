using System.Text;
using Gamex.DataObjects;
using Gamex.Mesh;
using OpenTK.Mathematics;
using Vector3 = System.Numerics.Vector3;

namespace Gamex.Model;

public class PointLight
{
  public const uint _buffSize = 256;
  private readonly byte[] _buffer = new byte[_buffSize];
  private CubeMesh _m = new();

  public string Name => Encoding.UTF8.GetString(_buffer);
  public Vector3 Location = Vector3.Zero;
  public Vector3 Color = Vector3.One;
  public float Scale => _m.Scale;

  public PointLight()
  {
    const string dName = "Default";
    for (var i = 0; i < dName.Length; i++)
    {
      _buffer[i] = (byte) dName[i];
    }
  }

  public byte[] Buffer => _buffer;

  public void Render(Matrix4 proj)
  {
    _m.Loc = LinearMath.ToTkVector3(Location);
    _m.Render(proj, LinearMath.ToTkVector3(Color));
  }
}
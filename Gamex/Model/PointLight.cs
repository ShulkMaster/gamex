using System.Numerics;
using System.Text;
using ObjLoader.Loader.Data;

namespace Gamex.Model;

public class PointLight
{
  public const uint _buffSize = 256;
  private readonly byte[] _buffer = new byte[_buffSize];

  public string Name => Encoding.UTF8.GetString(_buffer);
  public Vector3 Location = Vector3.Zero;
  public Vector3 Color = Vector3.One;

  public PointLight()
  {
    const string dName = "Default";
    for (var i = 0; i < dName.Length; i++)
    {
      _buffer[i] = (byte) dName[i];
    }
  }

  public byte[] Buffer => _buffer;
}
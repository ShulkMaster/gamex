using OpenTK.Mathematics;
using V3 = System.Numerics.Vector3;
using V2 = System.Numerics.Vector2;

namespace Gamex.DataObjects;

public static class LinearMath
{
  public static Vector2 ToTkVector2(V2 v)
  {
    return new Vector2(v.X, v.Y);
  }

  public static Vector3 ToTkVector3(V3 v)
  {
    return new Vector3(v.X, v.Y, v.Z);
  }

  public static V3 ToSysVector3(Vector3 v)
  {
    return new V3(v.X, v.Y, v.Z);
  }
}
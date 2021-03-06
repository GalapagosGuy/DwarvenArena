using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorCast
{
    /// <summary>
    /// Casts Vec3 to Vec2, discarding Y
    /// </summary>
    /// <returns></returns>
    public static Vector2 CastVector3ToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}

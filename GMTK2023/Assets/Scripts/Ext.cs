using UnityEngine;
public static class Ext
{
    public static Vector2 ToV2(this Vector3 v3)
        => new Vector2(v3.x, v3.y);
    public static float DistanceTo(this Transform transform, Vector3 to)
        => (transform.position.ToV2() - to.ToV2()).magnitude;
}
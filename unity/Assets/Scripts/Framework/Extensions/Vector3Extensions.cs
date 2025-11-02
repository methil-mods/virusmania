using UnityEngine;

namespace Framework.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 AddX(this Vector3 v, float value)
        {
            return new Vector3(v.x + value, v.y, v.z);
        }

        public static Vector3 AddY(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y + value, v.z);
        }
    }
}
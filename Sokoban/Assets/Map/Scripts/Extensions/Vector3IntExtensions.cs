using UnityEngine;

namespace Extensions
{
    public static class Vector3IntExtensions
    {
        public static Vector3 ToVector3(this Vector3Int vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
    }

}

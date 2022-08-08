using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3Int ToVector3Int(this Vector3 vector)
        {
            return new Vector3Int((int)Mathf.Round(vector.x), (int)Mathf.Round(vector.y), (int)Mathf.Round(vector.z));
        }
    }

}

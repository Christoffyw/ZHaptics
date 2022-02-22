using UnityEngine;

namespace ZHaptics
{
    public class Logging
    {
        public static string StringifyVector3(Vector3 value)
        {
            return $"{value.x} {value.y} {value.z}";
        }
    }
}
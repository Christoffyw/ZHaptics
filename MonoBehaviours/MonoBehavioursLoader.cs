using BepInEx;
using ZHaptics.MonoBehaviours;
using UnityEngine;

namespace ZHaptics
{
    public class MonoBehavioursLoader
    {
        private static bool injectionDone = false;
        public static void Inject()
        {
            if (injectionDone)
            {
                Debug.LogError("MonoBehaviours were already injected.");
            }
        }
    }
}
using HarmonyLib;
using Zenith.Locomotion;

namespace ZHaptics.Patches
{
    [HarmonyPatch(typeof(ZenithLocomotionManager), "Start")]
    public class ZenithLocomotionManager_Start
    {
        public static void Prefix(ZenithLocomotionManager __instance)
        {
            //__instance.onProvidersUpdated += Plugin.OnProvidersUpdated;
        }
    }
}
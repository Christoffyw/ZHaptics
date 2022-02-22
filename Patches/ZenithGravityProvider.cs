using HarmonyLib;
using BepInEx;
using Zenith.Locomotion;
using ZHaptics.Helpers;

namespace ZHaptics.Patches
{
    [HarmonyPatch(typeof(ZenithGlidingProvider), "HandleGliding")]
    public class HandleGliding
    {
        public static void Postfix()
        {
            MovementHelper.UpdateState(MovementState.Gliding);
        }
    }

    [HarmonyPatch(typeof(ZenithGravityProvider), "ProcessFallDamage")]
    public class ProcessFallDamage
    {
        public static void Prefix()
        {
            DamageHelper.AssertNextSource(DamageSource.Fall);
        }
        
        public static void Postfix()
        {
            DamageHelper.AssertNextSource(DamageSource.Unknown);
        }
    }
}
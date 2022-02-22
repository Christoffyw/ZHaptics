using System.Reflection;
using HarmonyLib;
using BepInEx;
using UnityEngine;
using Zenith.Locomotion;
using ZHaptics.Haptics.Patterns;
using ZHaptics.Helpers;

namespace ZHaptics.Patches
{
    [HarmonyPatch(typeof(Zenith.Locomotion.ZenithGravityProvider), "OnProvidersUpdated")]
    public class OnProvidersUpdated
    {
        public static ZenithClimbingProvider climber;
        public static ZenithGlidingProvider glider;
        public static void Postfix(ZenithGravityProvider __instance)
        {
            climber = ZenithLocomotionManager.instance.GetProvider<ZenithClimbingProvider>();
            glider = ZenithLocomotionManager.instance.GetProvider<ZenithGlidingProvider>();
            
            FlyingAir.Init((Transform)typeof(ZenithGlidingProvider).GetField("centerGlideHelper", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(glider));
            FallingAir.Init(__instance);
            CombatSystemHelper.Init();
        }
    }
    
    [HarmonyPatch(typeof(Zenith.Locomotion.ZenithGravityProvider), "OnUpdate")]
    public class UpdateTransform
    {
        private static FieldInfo falling =
            typeof(ZenithGravityProvider).GetField("previouslyFalling", BindingFlags.Instance | BindingFlags.NonPublic);
        public static void Postfix(Zenith.Locomotion.ZenithGravityProvider __instance)
        {
            if (OnProvidersUpdated.climber.isClimbing || OnProvidersUpdated.glider.isGliding)
                return;
            
            MovementHelper.UpdateState((bool) falling.GetValue(__instance) ? MovementState.Falling : MovementState.Idle);
        }
    }
}
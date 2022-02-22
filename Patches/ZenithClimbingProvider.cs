using HarmonyLib;
using UnityEngine;
using Zenith.Locomotion;
using ZHaptics.Haptics.Patterns;
using ZHaptics.Helpers;

namespace ZHaptics.Patches
{
    [HarmonyPatch(typeof(ZenithClimbingProvider), "BeginClimb")]
    public class BeginClimb
    {
        public static void Postfix(ZenithClimberHelper climber, Transform target)
        {
            MovementHelper.UpdateState(MovementState.Climbing);
            Climbing.Execute(climber.grabber.HandSide);
        }
    }
}
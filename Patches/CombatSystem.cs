using System;
using System.Reflection;
using HarmonyLib;
using Improbable.Gdk.Core;
using Zenith;
using ZHaptics.Helpers;

namespace ZHaptics.Patches
{
    [HarmonyPatch(typeof(CombatSystem), "DoDamage")]
    public class DoDamage
    {
        public static void Prefix(
            EntityId target,
            int amount,
            EventLocation location,
            bool bypassVulnerability,
            Action<ActorStatsComponent.Damage.ReceivedResponse> OnTargetKilled = null,
            string itemInstanceId = "",
            string abilityName = null,
            float? critChanceAddition = null,
            float? critMultiplierAddition = null,
            DamageType damageType = DamageType.normal)
        {
            if (target != PlayerCharacterCommandSystem.instance.entityId)
                return;
            
            DamageHelper.OnDamage(amount, location, bypassVulnerability);
        }
    }
}
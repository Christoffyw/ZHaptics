using System.Reflection;
using BepInEx;
using UnityEngine;
using Zenith;
using ZHaptics.ConfigManager;
using ZHaptics.Haptics.EffectHelpers;

namespace ZHaptics.Haptics.Patterns
{
    public class MeleeDamage
    {
        public static void Execute(GameObject attacker, Vector3 hitPos, Vector3 normal, int damage,
            bool rangedFix = false)
        {
            var sourceDirection = (attacker.transform.position - hitPos).normalized;
            var player = GameObjectTrackerSystem.TryGetEntityToClientGameObject(new Il2CppSystem.Nullable<Improbable.Gdk.Core.EntityId>(PlayerCharacterCommandSystem.instance.entityId));
            
            var angle = BhapticsUtils.Angle(player.transform.forward, sourceDirection);
            
            PlayerHit(angle, damage, rangedFix);
        }
        
        public static void PlayerHit(float angle, int damage, bool rangedMode)
        {
            var stats = PlayerCharacterCommandSystem.instance.actorStatsReader.Data.TotalStats;
            if (!stats.ContainsKey(StatsType.health))
            {
                Debug.Log("Cannot find hp");
            }

            var health = stats[StatsType.health];
            
            float damageRatio = (float)Mathf.Clamp(damage, 0, health) / health;
            if (rangedMode)
            {
                EffectPlayer.Play("Vest/BulletHit_Level2", new Effect.EffectProperties
                {
                    XRotation = -angle,
                    Time = 0.5f
                });
            }
            else
            {
                EffectPlayer.Stop("Vest/BulletHit_Level2");
                if (DynConfig.Toggles.Vest.MeleeHit)
                    EffectPlayer.Play("Vest/MeleeDamage", new Effect.EffectProperties
                    {
                        XRotation = -angle,
                        Strength = Mathf.Lerp(0.6f, 1f, damageRatio),
                        Time = 0.7f
                    });
            }
        }
    }
}
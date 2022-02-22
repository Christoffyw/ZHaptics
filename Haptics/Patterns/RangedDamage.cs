using BepInEx;
using UnityEngine;
using Zenith;
using ZHaptics.ConfigManager;
using ZHaptics.Haptics.EffectHelpers;

namespace ZHaptics.Haptics.Patterns
{
    public class RangedDamage
    {
        public static void Execute(GameObject attacker, Vector3 hitPos, Vector3 normal, int damage)
        {
            var player = GameObjectTrackerSystem.TryGetEntityToClientGameObject(new Il2CppSystem.Nullable<Improbable.Gdk.Core.EntityId>(PlayerCharacterCommandSystem.instance.entityId));
            
            Vector3 forward = Quaternion.Euler(0, -90f, 0) * player.transform.forward;
            var angle = BhapticsUtils.Angle(forward, -normal);

            // float offsetY = Mathf.Clamp((info.ImpactPosition.y - (vestRef.position.y + PatternManager.VestCenterOffset)) / PatternManager.VestHeight, -0.5f, 0.5f);
            
            PlayerHit(angle);
        }
        
        public static void PlayerHit(float angle)
        {
            if (DynConfig.Toggles.Vest.BulletHit)
                EffectPlayer.Play("Vest/BulletHit_Level2", new Effect.EffectProperties
                {
                    XRotation = angle
                });
        }
    }
}
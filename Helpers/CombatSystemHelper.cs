using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core;
using Improbable.Gdk.Standardtypes;
using Improbable.Gdk.StandardTypes;
using Improbable.Gdk.Subscriptions;
using BepInEx;
using UnityEngine;
using Zenith;
using ZHaptics.Haptics.Patterns;

namespace ZHaptics.Helpers
{
    public class CombatSystemHelper
    {
        public static bool Initialized = false;
        public static CombatComponentReader Reader = null;

        public static void Init()
        {
            var combatSystem = PlayerCharacterCommandSystem.instance.GetSpatialSystem<CombatSystem>();
            var id = PlayerCharacterCommandSystem.instance.entityId;

            var playerObject = GameObjectTrackerSystem.TryGetEntityToClientGameObject(new Il2CppSystem.Nullable<EntityId>(id));
            if (playerObject == null)
            {
                Debug.Log("Failed to get player");
                return;
            }

            var reader = typeof(CombatSystem).GetField("combatComponentReader",
                BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(combatSystem);

            if (reader == null)
            {
                Debug.Log("Cannot get combat system");
                return;
            }

            Reader = (CombatComponentReader)reader;
            Initialized = true;

            Action<TakeDamageEvent> onTakeDamageAction = OnTakeDamage;
            Action<DamageEvent> onDamageAction = OnDamage;
            Reader.add_OnTakeDamageEvent(onTakeDamageAction);
            combatSystem.onSuccessfulDamage += onDamageAction;
        }

        private static void OnDamage(DamageEvent data)
        {
            var source = GameObjectTrackerSystem.TryGetEntityToClientGameObject(new Il2CppSystem.Nullable<EntityId>(new EntityId(Int64.Parse(data.ActorEntityId ?? "0"))));

            if (source == null)
                return;
            
            var projectile = source.GetComponent<ClientProjectileSystem>();
            //var configs = (List<ZenithEnemyRangedAttackConfig>) typeof(ClientProjectileSystem)
            //    .GetField("rangedAttackConfigs", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(projectile);
            //if (configs != null && configs.Count > 0)
            //    MeleeDamage.Execute(
            //        source, 
            //        PlayerCharacterCommandSystem.instance.transform.position, 
            //        Vector3.zero, 
            //       data.Amount,
            //       true);                        ZenithEnemyRangedAttackConfig does not exist__instance
        }

        private static void OnTakeDamage(TakeDamageEvent data)
        {
            var source = GameObjectTrackerSystem.TryGetEntityToClientGameObject(new Il2CppSystem.Nullable<EntityId>(data.AttackInfo.AttackerEntityId));
            var target = GameObjectTrackerSystem.TryGetEntityToClientGameObject(new Il2CppSystem.Nullable<EntityId>(data.AttackInfo.TargetEntityId));

            switch (data.AttackInfo.CombatType)
            {
                case CombatType.RANGED:
                    RangedDamage.Execute(
                        source, 
                        Vector3Extensions.ToVector3(data.EventLocation.Origin), 
                        Vector3Extensions.ToVector3(data.EventLocation.Normal.GetValueOrDefault(Vector3Extensions.ToIntAbsolute(Vector3.zero))), 
                        data.AttackInfo.DamageAmount.GetValueOrDefault(0));
                    break;
                case CombatType.MELEE:
                    MeleeDamage.Execute(
                        source, 
                        Vector3Extensions.ToVector3(data.EventLocation.Origin), 
                        Vector3Extensions.ToVector3(data.EventLocation.Normal.GetValueOrDefault(Vector3Extensions.ToIntAbsolute(Vector3.zero))), 
                        data.AttackInfo.DamageAmount.GetValueOrDefault(0));
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using UnityEngine;
using Zenith;
using ZHaptics.Haptics.EffectHelpers;

namespace ZHaptics.Helpers
{
    public class StaminaHelper
    {
        private static bool _initalized = false;
        public static ClientStaminaSystem _staminaSystem;
        private static float lastStamina = 0f;

        private const float heartbeatEffectThreashold = 0.6f;
        private const float heartbeatEffectThreasholdRecharging = 1f;
        private const float heartbeatMinInterval = 0.6f;
        private const float heartbeatMaxInterval = 1f;
        private const float heartbeatStrengthMin = 0.1f;
        private const float heartbeatStrengthMax = 0.8f;
        private static bool consumingStamina = false;

        private const float breathResetTarget = 0.8f;
        private const float breathMinWait = 0f;
        private const float breathMaxWait = 1f;
        private const float breathStrengthMin = 0.05f;
        private const float breathStrengthMax = 0.1f;
        private const float breathRateMin = 0.2f;
        private const float breathRateMax = 0.5f;
        
        private static bool heartbeatTriggered = false;
        private static DateTime lastHeartbeat = new DateTime(0);

        private static bool hasStaminaUpdatedThisUpdate = false;
        private static bool breathingStarted = false;
        private static DateTime lastCompleteBreath = new DateTime(0);

        public static void OnFixedUpdate()
        {
            if (!_initalized)
                return;

            consumingStamina = lastStamina > _staminaSystem.currentStamina;
            hasStaminaUpdatedThisUpdate = Mathf.Approximately(lastStamina, _staminaSystem.currentStamina);
            
            HeartbeatEffect();
            // BreathEffect();
        }

        private static void BreathEffect()
        {
            if (!heartbeatTriggered && !breathingStarted)
                return;

            if (!hasStaminaUpdatedThisUpdate)
            {
                breathingStarted = true;
            }
            
            if (!breathingStarted)
                return;

            var ratio = _staminaSystem.currentStamina / _staminaSystem.maxStamina;
            if (ratio >= breathResetTarget)
            {
                breathingStarted = false;
                return;
            }
            
            var threasholdRatio = Mathf.Clamp(ratio / breathResetTarget, 0f, 1f);
            
            var timeSpacing =
                Mathf.Lerp(breathMinWait, breathMaxWait, threasholdRatio);
            var strength =
                Mathf.Lerp(breathStrengthMax, breathStrengthMin, threasholdRatio);
            var time = 
                Mathf.Lerp(breathRateMin, breathRateMax, threasholdRatio);
            
            TimeSpan ts = DateTime.Now - lastCompleteBreath;
            var elapsedTime = (float)ts.TotalSeconds;
            
            if (elapsedTime < timeSpacing)
                return;
            
            EffectPlayer.Play("Vest/Breath", new Effect.EffectProperties
            {
                Strength = strength,
                Time = time,
                OnComplete = () =>
                {
                    lastCompleteBreath = DateTime.Now;
                }
            });
        }
        
        private static void HeartbeatEffect()
        {
            var ratio = _staminaSystem.currentStamina / _staminaSystem.maxStamina;

            if (!heartbeatTriggered && ratio > heartbeatEffectThreashold)
                return;

            var threashold = consumingStamina ? heartbeatEffectThreashold : heartbeatEffectThreasholdRecharging;
            if (ratio >= threashold)
            {
                heartbeatTriggered = false;
                return;
            }

            heartbeatTriggered = true;

            var threasholdRatio = Mathf.Clamp(ratio / threashold, 0f, 1f);
            
            var interval =
                Mathf.Lerp(heartbeatMinInterval, heartbeatMaxInterval, threasholdRatio);
            var strength =
                Mathf.Lerp(heartbeatStrengthMax, heartbeatStrengthMin, threasholdRatio) * (consumingStamina ? 1f : 0.7f);
            
            TimeSpan ts = DateTime.Now - lastHeartbeat;
            var elapsedTime = (float)ts.TotalSeconds;
            
            if (elapsedTime < interval)
                return;

            EffectPlayer.Play("Vest/HeartPulse", new Effect.EffectProperties
            {
                Strength = strength
            });
            
            lastHeartbeat = DateTime.Now;
        }

        public static void Init()
        {
            if (_initalized)
                return;

            _initalized = true;
            
            var player = PlayerCharacterCommandSystem.instance;
            _staminaSystem = player.GetSpatialSystem<ClientStaminaSystem>();
        }
        
        
        public static void OnStaminaUpdate()
        {
            if (_staminaSystem.currentStamina < lastStamina)
                heartbeatTriggered = false;
            
            lastStamina = _staminaSystem.currentStamina;
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(ClientStaminaSystem), "OnTotalStatsUpdate")]
    class StaminaUpdate
    {
        static public void Postfix(ClientStaminaSystem __instance, Dictionary<StatsType, int> totalStats)
        {
            if(__instance == StaminaHelper._staminaSystem)
                StaminaHelper.OnStaminaUpdate();
        }
    }
}
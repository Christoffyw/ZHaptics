using System;
using Bhaptics.Tact;
using BepInEx;
using ZHaptics.ConfigManager;
using ZHaptics.Haptics.EffectHelpers;

namespace ZHaptics.Haptics.Patterns
{
    public class FallDamage
    {
        public static void Execute(int damage, float force)
        {
            if (damage >= 200)
            {
                if (DynConfig.Toggles.Vest.FallDamage)
                    EffectPlayer.Play("Vest/FallDamage_Level2");
                
                if (DynConfig.Toggles.Feet.FallDamage)
                    EffectPlayer.Play("Foot/FallDamage");
            }
            else if (damage >= 100)
            {
                if (DynConfig.Toggles.Vest.FallDamage)
                    EffectPlayer.Play("Vest/FallDamage_Level1");
                
                if (DynConfig.Toggles.Feet.FallDamage)
                    EffectPlayer.Play("Foot/FallDamage", new Effect.EffectProperties
                    {
                        Strength = 0.6f
                    });
            } else {
                if (DynConfig.Toggles.Vest.FallDamage)
                    EffectPlayer.Play("Vest/FallDamage_Level1", new Effect.EffectProperties
                    {
                        Strength = 0.7f
                    });
                
                if (DynConfig.Toggles.Feet.FallDamage)
                    EffectPlayer.Play("Foot/FallDamage", new Effect.EffectProperties
                    {
                        Strength = 0.4f
                    });
            }
        }
    }
}
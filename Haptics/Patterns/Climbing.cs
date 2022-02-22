using BNG;
using ZHaptics.ConfigManager;
using ZHaptics.Haptics.EffectHelpers;

namespace ZHaptics.Haptics.Patterns
{
    public class Climbing
    {
        public static void Execute(ControllerHand value)
        {
            if (DynConfig.Toggles.Arms.Climbing)
                EffectPlayer.Play($"Arm/Climbing{HapticUtils.HandExt(value)}");
            
            if (DynConfig.Toggles.Hands.Climbing)
                EffectPlayer.Play($"Hand/Climbing{HapticUtils.HandExt(value)}");
            
            if (DynConfig.Toggles.Vest.Climbing)
                EffectPlayer.Play($"Vest/Climbing{HapticUtils.HandExt(value)}");
        }
    }
}
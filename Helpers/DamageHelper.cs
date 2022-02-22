using BepInEx;
using Zenith;
using ZHaptics.Haptics.Patterns;

namespace ZHaptics.Helpers
{
    public enum DamageSource
    {
        Unknown,
        Fall
    }
    
    public class DamageHelper
    {
        private static DamageSource expectedSource = DamageSource.Unknown;

        public static void AssertNextSource(DamageSource next)
        {
            expectedSource = next;
        }

        public static void OnDamage(int amount, EventLocation source, bool bypassVulnerability)
        {
            switch (expectedSource)
            {
                case DamageSource.Fall:
                    OnFallDamage(amount);
                    break;
            }

            expectedSource = DamageSource.Unknown;
        }

        private static void OnFallDamage(int amount)
        {
            FallDamage.Execute(amount, 0.0f);
        }
    }
}
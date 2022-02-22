using HarmonyLib;
using Zenith;
using ZHaptics.Helpers;

namespace ZHaptics.Patches
{
    [HarmonyPatch(typeof(PlayerCharacterCommandSystem), "OnEnable")]
    public class PlayerCharacterCommandSystem_OnEnable
    {
        public static void Postfix(PlayerCharacterCommandSystem __instance)
        {
            StaminaHelper.Init();
        }
    }
}
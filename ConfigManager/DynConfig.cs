using ZHaptics.ConfigManager.ConfigElements;
using ZHaptics.ConfigManager.ConfigElements.EffectToggles;

namespace ZHaptics.ConfigManager
{
    public class DynConfig
    {
        public enum SceneMode
        {
            General,
            Lobby
        }
        
        public static EffectToggles Toggles = EffectToggles.DefaultConfig;
        public static SceneMode Mode { get; private set; } = SceneMode.General;

        public static void UpdateConfig(SceneMode mode)
        {
            Mode = mode;
            
            if (mode == SceneMode.General)
                Toggles = ConfigLoader.Config.EffectToggles.General;
            else if (mode == SceneMode.Lobby)
                Toggles = ConfigLoader.Config.EffectToggles.Lobby;
        }
    }
}
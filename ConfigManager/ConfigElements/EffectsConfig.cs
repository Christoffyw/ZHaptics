using YamlDotNet.Serialization;

namespace ZHaptics.ConfigManager.ConfigElements
{
    public class EffectsConfig
    {
        public FlyEffects Flying { get; set; }
        
        [YamlIgnore] 
        public static EffectsConfig DefaultConfig = new EffectsConfig
        {
            Flying = FlyEffects.DefaultConfig
        };
    }
}
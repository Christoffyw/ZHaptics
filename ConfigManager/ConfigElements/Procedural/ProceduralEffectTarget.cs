using YamlDotNet.Serialization;

namespace ZHaptics.ConfigManager.ConfigElements
{
    public class ProceduralEffectTarget : ProceduralEffectBase
    {
        public LerpGoal Goal { get; set; }
    }
}
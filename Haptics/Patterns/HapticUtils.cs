using BNG;

namespace ZHaptics.Haptics.Patterns
{
    public class HapticUtils
    {
        public static string HandExt(ControllerHand hand)
        {
            if (hand == ControllerHand.Left)
            {
                return "_L";
        
            } else if (hand == ControllerHand.Right)
            {
                return "_R";
            }
        
            return "";
        }
    }
}
using BepInEx;
using ZHaptics.Haptics.Patterns;

namespace ZHaptics.Helpers
{
    public enum MovementState
    {
        None,
        Gliding,
        Walking,
        Running,
        Idle,
        Falling,
        Climbing
    }
    
    public class MovementHelper
    {
        public static MovementState previousState = MovementState.None;
        public static MovementState state = MovementState.None;

        public static void UpdateState(MovementState status)
        {
            if (state == status)
                return;

            previousState = state;
            state = status;
                
            var flyingOrGliding = (state == MovementState.Falling && previousState == MovementState.Gliding) ||
                                  (state == MovementState.Gliding && previousState == MovementState.Falling);

            if (!flyingOrGliding)
            {
                FallingAir.Clear();
                FlyingAir.Clear();
            }
        }
        
        public static void OnFixedUpdate()
        {
            if (MovementHelper.state == MovementState.None)
                return;

            switch (MovementHelper.state)
            {
                case MovementState.Gliding:
                    FlyingAir.Execute(MovementHelper.previousState == MovementState.Falling);
                    break;
                case MovementState.Falling:
                    FallingAir.Execute(MovementHelper.previousState == MovementState.Gliding);
                    break;
            }
        }
    }
}
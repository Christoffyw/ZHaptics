using UnityEngine;
using UnityEngine.Serialization;

namespace ZHaptics.MonoBehaviours
{
    public class DebugMarker : MonoBehaviour
    {
        public LineDrawer DebugLine;

        public float Size;

        // public Collider Collider;
        
        private void OnEnable()
        {
            // Collider = GetComponent<Collider>();
            DebugLine = new LineDrawer();
        }

        private void FixedUpdate()
        {
            var position = transform.position;
            DebugLine.DrawLineInGameView(position + new Vector3(0, -(Size / 2f), 0), position + new Vector3(0, Size / 2f, 0), Color.blue);
        }

        private void OnDestroy()
        {
            DebugLine.Destroy();
        }

        public void SetSize()
        {
            DebugLine.Destroy();
            DebugLine = new LineDrawer(Size);
        }
    }
}
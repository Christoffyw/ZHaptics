using System;
using UnityEngine;

namespace ZHaptics
{
    public class VelocityTracker : MonoBehaviour
    {
        private Vector3 _current = Vector3.zero;
        private Vector3 _previous = Vector3.zero;

        private Vector3 _velocity = Vector3.zero;
        public Vector3 Velocity => _velocity;

        private void FixedUpdate()
        {
            _current = transform.position;
            _velocity = (_previous - _current) / Time.deltaTime;
            _previous = _current;
        }
    }
}
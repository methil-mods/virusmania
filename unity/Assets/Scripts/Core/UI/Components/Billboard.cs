using UnityEngine;

namespace Core.UI
{
    public class Billboard : MonoBehaviour
    {
        [Tooltip("Target to look at (if null => Main Camera)")]
        public Transform target = null;

        [Tooltip("If true, the object will face away from the target")]
        public bool inverse = false;

        [Tooltip("If true, the object will remain upright (ignore X rotation)")]
        public bool keepUpright = false;

        void Awake()
        {
            if (target == null && Camera.main != null)
            {
                target = Camera.main.transform;
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // Direction vector: either look at or look away
            Vector3 direction = inverse ? transform.position - target.position : target.position - transform.position;

            // Optional: keep upright
            if (keepUpright)
                direction.y = 0;

            if (direction.sqrMagnitude > 0.001f) // Avoid zero-length vector
                transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
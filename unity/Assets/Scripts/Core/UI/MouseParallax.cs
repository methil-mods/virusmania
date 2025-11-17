using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.UI
{
    public class MouseParallax : MonoBehaviour
    {
        [SerializeField] private float intensity = 10f;
        [SerializeField] private Transform secondaryTarget;
        [SerializeField] private float idleAmplitude = 0.05f;
        [SerializeField] private float idleSpeed = 1f;

        private Vector3 basePos;
        private Vector3 secondaryBasePos;

        void Start()
        {
            basePos = transform.localPosition;
            if (secondaryTarget != null) secondaryBasePos = secondaryTarget.localPosition;
        }

        void Update()
        {
            if (Mouse.current == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 mouseNorm = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
            mouseNorm = (mouseNorm * 2f) - Vector2.one;

            Vector3 offset = new Vector3(mouseNorm.x, mouseNorm.y, 0f) * intensity;
            float idleY = Mathf.Sin(Time.time * idleSpeed) * idleAmplitude;
            transform.localPosition = basePos + offset + new Vector3(0f, idleY, 0f);

            if (secondaryTarget != null)
            {
                Vector3 offset2 = offset / 1.5f;
                secondaryTarget.localPosition = secondaryBasePos + offset2;
            }
        }
    }
}
using System;
using Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    [Serializable]
    public class PlayerMovement : Updatable<PlayerController>
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private InputActionReference moveAction;

        private Rigidbody rb;
        private Vector3 currentVelocity;

        // ✅ Variable publique pour la direction
        [NonSerialized] public Vector3 direction = Vector3.zero;

        public override void Start(PlayerController controller)
        {
            moveAction.action.Enable();
            rb = controller.body.GetComponent<Rigidbody>();
            if (rb == null) rb = controller.body.gameObject.AddComponent<Rigidbody>();
        }

        public override void FixedUpdate(PlayerController controller)
        {
            Vector2 input = moveAction.action.ReadValue<Vector2>();
            Vector3 targetVelocity = new Vector3(input.x, 0, input.y) * speed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            Vector3 velocityChange = currentVelocity - rb.linearVelocity;
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            // ✅ Met à jour la direction
            if (currentVelocity.sqrMagnitude > 0.001f)
                direction = currentVelocity.normalized;
        }

        public override void OnDrawGizmos(PlayerController controller)
        {
            Gizmos.color = Color.cyan;

            Vector3 origin = controller.body.transform.position;
            Vector3 end = origin + direction * 2f;
            Gizmos.DrawLine(origin, end);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 150, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -150, 0) * Vector3.forward;
            Gizmos.DrawLine(end, end + right * 0.5f);
            Gizmos.DrawLine(end, end + left * 0.5f);
        }
    }
}

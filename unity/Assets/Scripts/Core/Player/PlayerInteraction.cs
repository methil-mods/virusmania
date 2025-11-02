using System;
using Framework;
using Framework.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    [Serializable]
    public class PlayerInteraction : Updatable<PlayerController>
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private float interactionRadius = 0.5f;
        [SerializeField] private InputActionReference interactionAction;

        public override void Start(PlayerController controller)
        {
            _playerMovement = controller.updatables.FirstOfType<PlayerMovement>();
            if (_playerMovement == null) Debug.LogError("No player movement found in PlayerController");
            if (interactionAction == null) Debug.LogError("No player interaction action set in PlayerInteraction");
            else
            {
                interactionAction.action.performed += _ => Interact();
                interactionAction.action.Enable();
            }
        }

        public override void Update(PlayerController controller)
        {
            var dir = _playerMovement.direction;
        }

        public void Interact()
        {
            // TODO : create temporary collider triggering box that check if 
            //  there is a gameobject that I can interact with (who inherit from interface) 
            Debug.LogWarning("Interacting...");
        }

        public override void OnDrawGizmos(PlayerController controller)
        {
            if (controller == null || controller.body == null || _playerMovement == null) return;
            Gizmos.color = Color.yellow;
            Vector3 origin = controller.body.transform.position;
            origin = origin.AddY(1.4f);
            Vector3 center = origin + _playerMovement.direction * interactionDistance;
            Gizmos.DrawWireSphere(center, interactionRadius);
        }
    }
}
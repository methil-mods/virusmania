using System;
using Core.Interaction;
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
        [Header("Interaction Settings")]
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private float interactionRadius = 0.5f;
        [Header("Interaction Input")]
        [SerializeField] private InputActionReference interactionAction;

        public override void Start(PlayerController controller)
        {
            _playerMovement = controller.updatables.FirstOfType<PlayerMovement>();
            if (_playerMovement == null) Debug.LogError("No player movement found in PlayerController");
            if (interactionAction == null) Debug.LogError("No player interaction action set in PlayerInteraction");
            else
            {
                interactionAction.action.performed += _ => Interact(controller);
                interactionAction.action.Enable();
            }
        }

        public override void Update(PlayerController controller)
        {
            
        }

        public void Interact(PlayerController controller)
        {
            Debug.LogWarning("Interacting...");
            
            Vector3 origin = controller.body.transform.position;
            origin = origin.AddY(1.4f);
            Vector3 center = origin + _playerMovement.direction * interactionDistance;

            Collider[] hits = Physics.OverlapSphere(center, interactionRadius);
            foreach (var hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
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
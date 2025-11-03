using System;
using Core.Interaction;
using Core.Item;
using Framework;
using Framework.Extensions;
using JetBrains.Annotations;
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
        [SerializeField] private InputActionReference interactionHoldAction;

        private HoldItem _holdingItem;
        private bool _isInteractingHeld;

        public bool HasItem => _holdingItem != null;

        public override void Start(PlayerController controller)
        {
            _playerMovement = controller.updatables.FirstOfType<PlayerMovement>();
            if (_playerMovement == null)
                Debug.LogError("No player movement found in PlayerController");

            if (interactionAction == null || interactionHoldAction == null)
            {
                Debug.LogError("Missing interaction actions in PlayerInteraction");
                return;
            }

            interactionAction.action.performed += ctx => Interact(controller);
            interactionAction.action.Enable();

            interactionHoldAction.action.performed += ctx => _isInteractingHeld = true;
            interactionHoldAction.action.canceled += ctx => _isInteractingHeld = false;
            interactionHoldAction.action.Enable();
        }

        public override void Update(PlayerController controller)
        {
            if (_isInteractingHeld)
                InteractHold(controller);
        }

        public void InteractHold(PlayerController playerController)
        {
            Vector3 origin = playerController.body.transform.position.AddY(1.4f);
            Vector3 center = origin + _playerMovement.direction * interactionDistance;

            Collider[] hits = Physics.OverlapSphere(center, interactionRadius);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IInteractable>(out var holdable))
                    holdable.InteractHold(playerController);
            }
        }

        public void Interact(PlayerController controller)
        {
            Vector3 origin = controller.body.transform.position.AddY(1.4f);
            Vector3 center = origin + _playerMovement.direction * interactionDistance;

            Collider[] hits = Physics.OverlapSphere(center, interactionRadius);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IInteractable>(out var interactable))
                    interactable.Interact(controller);
            }
        }

        public HoldItem RemoveItem()
        {
            var item = _holdingItem;
            _holdingItem = null;
            return item;
        }

        public bool GiveItem(HoldItem newItem)
        {
            if (_holdingItem != null) return false;
            _holdingItem = newItem;
            return true;
        }

        public override void OnDrawGizmos(PlayerController controller)
        {
            if (controller == null || controller.body == null || _playerMovement == null) return;

            Gizmos.color = Color.yellow;
            Vector3 origin = controller.body.transform.position.AddY(1.4f);
            Vector3 center = origin + _playerMovement.direction * interactionDistance;
            Gizmos.DrawWireSphere(center, interactionRadius);

            if (_holdingItem?.Item == null) return;

#if UNITY_EDITOR
            GUIStyle style = new GUIStyle
            {
                normal = new GUIStyleState { textColor = Color.yellow },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            UnityEditor.Handles.Label(origin, _holdingItem.Item.itemName, style);
#endif
        }
    }
}

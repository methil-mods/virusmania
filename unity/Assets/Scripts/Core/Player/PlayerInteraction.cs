using System;
using Framework;
using Framework.Extensions;
using UnityEngine;

namespace Core.Player
{
    [Serializable]
    public class PlayerInteraction : Updatable<PlayerController>
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private float interactionRadius = 0.5f;

        public override void Start(PlayerController controller)
        {
            _playerMovement = controller.updatables.FirstOfType<PlayerMovement>();
            if (_playerMovement == null) Debug.LogError("No player movement found in PlayerController");
        }

        public override void Update(PlayerController controller)
        {
            var dir = _playerMovement.direction;
        }

        public override void OnDrawGizmos(PlayerController controller)
        {
            if (controller == null || controller.body == null || _playerMovement == null) return;
            Gizmos.color = Color.yellow;
            Vector3 origin = controller.body.transform.position;
            Vector3 center = origin + _playerMovement.direction * interactionDistance;
            Gizmos.DrawWireSphere(center, interactionRadius);
        }
    }
}
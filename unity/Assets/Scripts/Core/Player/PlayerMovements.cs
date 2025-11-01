using System;
using Framework;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerMovements : Updatable<PlayerController>
    {
        [SerializeField] private float speed;
        public override void Start(PlayerController contoller)
        {
            // Nothing to do in here
        }

        public override void Update(PlayerController controller)
        {
            controller.transform.position += controller.transform.forward * (speed * Time.deltaTime);
        }
    }
}
using Core.Player;
using UnityEngine;

namespace Core.Interaction
{
    public class SendItemButtonInteractable : Interactable
    {
        [SerializeField] private Animator sendItemButtonAnimator;
        
        public override void Interact(PlayerController playerController)
        {
            sendItemButtonAnimator.SetTrigger("Activate");
            Debug.Log("Interacting with " + playerController.gameObject.name);
        }

        public override void InteractHold(PlayerController playerController)
        {
            Debug.LogWarning("InteractHold -- Useless in SendItemButtonInteractable");
        }
    }
}
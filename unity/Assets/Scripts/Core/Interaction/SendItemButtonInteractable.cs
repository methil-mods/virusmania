using Core.Player;
using UnityEngine;

namespace Core.Interaction
{
    public class SendItemButtonInteractable : Interactable
    {
        [SerializeField] private Animator sendItemButtonAnimator;
        [SerializeField] private SendItemInteractable sendItemInteractable;
        
        public override void Interact(PlayerController playerController)
        {
            sendItemButtonAnimator.SetTrigger("Activate");
            if (sendItemInteractable == null)
            {
                Debug.LogError("SendItemInteractable is null, cannot launch animation.");
            }
            else
            {
                sendItemInteractable.SendItem();
            }
            Debug.Log("Interacting with " + playerController.gameObject.name);
        }

        public override void InteractHold(PlayerController playerController)
        {
            Debug.LogWarning("InteractHold -- Useless in SendItemButtonInteractable");
        }
    }
}
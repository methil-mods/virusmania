using Core.Item.Holder;
using Core.Player;
using Framework.Extensions;
using UnityEngine;

namespace Core.Interaction
{
    public class TrashInteractable : Interactable
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private AudioSource audioSource;
        
        public override void Interact(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;

            if (playerInteraction.HasItem)
            {
                HoldItem removedItem = playerInteraction.RemoveItem();
                animator.SetTrigger("TriggerTrash");
                audioSource.PlayOneShot(SFXDatabase.instance.triggerTrashClip);
            }
        }

        public override void InteractHold(PlayerController playerController)
        {
        }
    }
}
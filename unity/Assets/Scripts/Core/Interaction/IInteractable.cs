using Core.Player;

namespace Core.Interaction
{
    public interface IInteractable
    {
        public void Interact(PlayerController playerController);
        public void InteractHold(PlayerController playerController);
    }
}
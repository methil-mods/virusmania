using Core.Computer;
using Core.Player;
using UnityEngine;

namespace Core.Interaction
{
    public class ComputerInteractable : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController playerController)
        {
            ComputerInterface.Instance.OpenPanel();
        }

        public void InteractHold(PlayerController playerController)
        {
            
        }
    }
}
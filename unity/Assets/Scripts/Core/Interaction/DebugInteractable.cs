using Core.Player;
using UnityEngine;

namespace Core.Interaction
{
    /// <summary>
    /// Not more than just a debug class that show how it should work
    /// </summary>
    public class DebugInteractable : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController playerController)
        {
            Debug.Log("Interacting with " + gameObject.name);
        }
        
        public void InteractHold(PlayerController playerController)
        {
            Debug.Log("Interacting hold with " + gameObject.name);
        }
    }
}
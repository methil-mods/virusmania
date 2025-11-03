using UnityEngine;

namespace Core.Interaction
{
    /// <summary>
    /// Not more than just a debug class that show how it should work
    /// </summary>
    public class DebugInteractable : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Interacting with " + gameObject.name);
        }
    }
}
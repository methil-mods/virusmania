using UnityEngine;

namespace Core.Interaction.Cook
{
    public class CookAnimationRelay : MonoBehaviour
    {
        [SerializeField] private CookInteractable cookInteractable;
        
        public void PlayOpenSound()
        {
            cookInteractable.PlayOpenClip();
        }
    }
}
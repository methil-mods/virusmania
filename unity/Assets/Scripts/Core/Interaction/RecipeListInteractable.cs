using Core.MergeLibrary;
using Core.Player;

namespace Core.Interaction
{
    public class RecipeListInteractable : Interactable
    {
        public override void Interact(PlayerController playerController)
        {
            MergeLibraryInterface.Instance.OpenPanel();
            // Silence...
        }

        public override void InteractHold(PlayerController playerController)
        {
            // Silence...
        }
    }
}
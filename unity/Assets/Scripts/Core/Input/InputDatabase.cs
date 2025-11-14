using Framework.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    [CreateAssetMenu(fileName = "InputDatabase", menuName = "Input/InputDatabase")]
    public class InputDatabase : SingletonScriptableObject<InputDatabase>
    {
        [Header("Movement Input")]
        public InputActionReference moveAction;
        
        [Header("Interaction Input")]
        public InputActionReference interactionAction;
        public InputActionReference interactionHoldAction;

        public void DisableInputs()
        {
            Debug.Log("-- Disable Inputs --");
            moveAction.action.Disable();
            interactionAction.action.Disable();
            interactionHoldAction.action.Disable();
        }

        public void EnableInputs()
        {
            Debug.Log("-- Enable Inputs --");
            moveAction.action.Enable();
            interactionAction.action.Enable();
            interactionHoldAction.action.Enable();
        }
    }
}
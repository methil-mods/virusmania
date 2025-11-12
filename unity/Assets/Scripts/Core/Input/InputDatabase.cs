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
    }
}
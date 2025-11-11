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

#if UNITY_EDITOR
        protected void OnDrawGizmos()
        {
            Vector3 titlePos = transform.position + Vector3.up * 3f;
            GUIStyle titleStyle = new GUIStyle
            {
                normal = new GUIStyleState { textColor = Color.paleGreen },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            UnityEditor.Handles.Label(titlePos, gameObject.name, titleStyle);
        }
#endif
    }
}
using Core.Brief;
using Core.Player;
using Framework.Extensions;
using UnityEngine;
using Core.Item.Holder;
using Core.Timer;

namespace Core.Interaction
{
    public class SendItemInteractable : Interactable
    {
        public override void Interact(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;

            if (playerInteraction.HasItem)
            {
                HoldItem removedItem = playerInteraction.RemoveItem();
                SendItem(removedItem);
            }
        }

        private void SendItem(HoldItem sendItem)
        {
            if (BriefController.Instance.TryToCompleteBrief(sendItem))
            {
                TimerController.Instance.StopTimer();
                Debug.Log("Send Item -> Validate brief !");
            }
            else
            {
                Debug.Log("Send Item -> Brief not validated...");
            }
        }

        public override void InteractHold(PlayerController playerController)
        {
            // Debug.Log("Interacting hold with " + gameObject.name);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position + Vector3.up * 2f;
            GUIStyle style = new GUIStyle
            {
                normal = new GUIStyleState { textColor = Color.cyan },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            UnityEditor.Handles.Label(pos, "SendItem", style);
        }
#endif
    }
}

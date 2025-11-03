using System;
using Core.Item;
using Core.Player;
using Framework.Extensions;
using UnityEngine;

namespace Core.Interaction
{
    public class ItemHolderInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Item.Item startItem;
        public HoldItem HoldingItem;

        private void Start()
        {
            if (startItem != null)
            {
                HoldingItem = startItem.GetHoldItem();
            }
        }


        public void Interact(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction != null)
            {
                if (playerInteraction.HasItem)
                {
                    if (HoldingItem == null)
                    {
                        HoldItem removedItem = playerInteraction.RemoveItem();
                        this.HoldingItem = removedItem;
                    }
                }
                else
                {
                    if (HoldingItem != null)
                    {
                        bool givedItem = playerInteraction.GiveItem(HoldingItem);
                        if (givedItem) this.HoldingItem = null;
                    }
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            if (HoldingItem != null && HoldingItem.Item != null)
            {
                Vector3 position = transform.position + Vector3.up * 2f;
                GUIStyle style = new GUIStyle
                {
                    normal = new GUIStyleState { textColor = Color.yellow },
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };
#if UNITY_EDITOR
                UnityEditor.Handles.Label(position, HoldingItem.Item.itemName, style);
#endif
            }
        }
    }
}
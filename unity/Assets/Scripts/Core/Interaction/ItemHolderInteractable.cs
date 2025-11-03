using System;
using System.Collections.Generic;
using Core.Item;
using Core.Player;
using Framework.Extensions;
using UnityEngine;

namespace Core.Interaction
{
    public class ItemHolderInteractable : MonoBehaviour, IInteractable
    {
        [Header("Configuration")]
        [SerializeField] private int maxHoldableItems = 3; // nombre max d'items pouvant être déposés
        [SerializeField] private List<Item.Item> startItems; // liste des items de départ (optionnelle)

        [Header("Runtime")]
        public List<HoldItem> HoldingItems = new List<HoldItem>();

        private void Start()
        {
            // Initialise les items de départ
            if (startItems != null && startItems.Count > 0)
            {
                foreach (var item in startItems)
                {
                    if (item != null && HoldingItems.Count < maxHoldableItems)
                    {
                        HoldingItems.Add(item.GetHoldItem());
                    }
                }
            }
        }

        public void Interact(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;

            // Cas 1 : le joueur tient un objet → il essaye de le poser
            if (playerInteraction.HasItem)
            {
                if (HoldingItems.Count < maxHoldableItems)
                {
                    HoldItem removedItem = playerInteraction.RemoveItem();
                    if (removedItem != null)
                    {
                        HoldingItems.Add(removedItem);
                    }
                }
                else
                {
                    Debug.Log("Le support est plein !");
                }
            }
            // Cas 2 : le joueur n’a pas d’objet → il essaye d’en prendre un
            else
            {
                if (HoldingItems.Count > 0)
                {
                    HoldItem itemToGive = HoldingItems[^1]; // dernier item ajouté (pile LIFO)
                    bool givedItem = playerInteraction.GiveItem(itemToGive);
                    if (givedItem)
                    {
                        HoldingItems.RemoveAt(HoldingItems.Count - 1);
                    }
                }
            }
        }
        
        public void InteractHold(PlayerController playerController)
        {
            Debug.Log("Interacting hold with " + gameObject.name);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (HoldingItems != null && HoldingItems.Count > 0)
            {
                Vector3 basePos = transform.position + Vector3.up * 2f;
                GUIStyle style = new GUIStyle
                {
                    normal = new GUIStyleState { textColor = Color.yellow },
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };

                for (int i = 0; i < HoldingItems.Count; i++)
                {
                    var holdItem = HoldingItems[i];
                    if (holdItem?.Item != null)
                    {
                        Vector3 pos = basePos + Vector3.up * (0.3f * i);
                        UnityEditor.Handles.Label(pos, holdItem.Item.itemName, style);
                    }
                }
            }
#endif
        }
    }
}

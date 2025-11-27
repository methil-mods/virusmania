using System;
using Core.Item.Holder;
using Core.Player;
using Framework.Controller;
using Framework.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Item
{
    public class ItemHoldInterface : BaseController<ItemHoldInterface>
    {
        public GameObject itemHoldInterface;
        public Image holdItemImage;
        public TextMeshProUGUI holdItemName;
        
        public void Start()
        {
            var playerInteraction = PlayerController.Instance.updatables.FirstOfType<PlayerInteraction>();

            playerInteraction.OnItemAdded += UpdateInterface;
            playerInteraction.OnItemRemoved += HideInterface;
            
            itemHoldInterface.gameObject.SetActive(false);
        }

        public void OnDisable()
        {
            var playerInteraction = PlayerController.Instance.updatables.FirstOfType<PlayerInteraction>();
            
            playerInteraction.OnItemAdded -= UpdateInterface;
            playerInteraction.OnItemRemoved -= HideInterface;
        }

        private void HideInterface(HoldItem holdItem)
        {
            itemHoldInterface.gameObject.SetActive(false);
        }

        private void UpdateInterface(HoldItem holdItem)
        {
            itemHoldInterface.gameObject.SetActive(true);
            if (holdItem.Item.itemIcon != null)
            {
                holdItemImage.sprite = holdItem.Item.itemIcon;
            }
            holdItemName.text = holdItem.Item.itemName;
        }
    }
}
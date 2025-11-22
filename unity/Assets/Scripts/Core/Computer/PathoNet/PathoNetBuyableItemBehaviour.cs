using System;
using Core.Interaction;
using Core.Item;
using Core.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Core.Computer.PathoNet
{
    public class PathoNetBuyableItemBehaviour : MonoBehaviour
    {
        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemPrice;
        
        public TextMeshProUGUI itemAmount;
        public Button addItemButton;
        public Button removeItemButton;
        
        public Item.Item itemData;
        private PathoNetInterface _pathoInterface;
        public int amount = 0;

        private UnityAction _onUpdateInterface;
        
        public void Start()
        {
            amount = 0;
            addItemButton.onClick.AddListener(AddItem);
            removeItemButton.onClick.AddListener(RemoveItem);
        }

        public void Setup(Item.Item item, PathoNetInterface pathoInterface)
        {
            amount = 0;
            itemData = item;
            _pathoInterface = pathoInterface;

            if (itemImage != null)
                itemImage.sprite = item.itemIcon;

            if (itemName != null)
                itemName.text = item.itemName;

            if (itemPrice != null)
                itemPrice.text = "$" + item.price;
            
            _onUpdateInterface += pathoInterface.UpdateInterface;
            UpdateAmountText();
        }

        public void AddItem()
        {
            amount++;
            UpdateAmountText();
        }

        public void RemoveItem()
        {
            if(amount > 0) amount--;
            UpdateAmountText();
        }

        public void UpdateAmountText()
        {
            itemAmount.text = amount.ToString();
            _onUpdateInterface?.Invoke();
        }
    }
}
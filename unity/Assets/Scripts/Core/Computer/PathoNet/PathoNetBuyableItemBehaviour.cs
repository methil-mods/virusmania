using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.Computer.PathoNet
{
    public class PathoNetBuyableItemBehaviour : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemPrice;
        public TextMeshProUGUI itemAmount;
        public Button addItemButton;
        public Button removeItemButton;
        
        public Item.Item itemData;
        
        private PathoNetInterface _pathoInterface;
        [NonSerialized] public int Amount;
        private bool _isOnBoarding;
        private bool _boughtOnce;
        private UnityAction _onUpdateInterface;
        
        public void Start()
        {
            Amount = 0;
            addItemButton.onClick.AddListener(AddItem);
            removeItemButton.onClick.AddListener(RemoveItem);
        }

        public void Setup(Item.Item item, PathoNetInterface pathoInterface, bool isOnBoarding)
        {
            Amount = 0;
            itemData = item;
            _pathoInterface = pathoInterface;

            if (itemImage != null)
                itemImage.sprite = item.itemIcon;

            if (itemName != null)
                itemName.text = item.itemName;

            if (itemPrice != null)
                itemPrice.text = "$" + item.price;
            
            _onUpdateInterface += pathoInterface.UpdateInterface;
            _isOnBoarding = isOnBoarding;
            UpdateAmountText();
        }

        public void AddItem()
        {
            if (_isOnBoarding && Amount >= 1) return;
            if (_isOnBoarding && _boughtOnce) return;
            Amount++;
            UpdateAmountText();
        }

        public void RemoveItem()
        {
            if(Amount > 0) Amount--;
            UpdateAmountText();
        }

        public void UpdateAmountText()
        {
            itemAmount.text = Amount.ToString();
            _onUpdateInterface?.Invoke();
        }

        public void Buy()
        {
            for (int i = 0; i < Amount; i++)
            {
                _pathoInterface.pathoItemReceiver.AddItem(itemData.GetHoldItem());
                _pathoInterface.OnBuyItem?.Invoke(itemData);
            }
            
            _boughtOnce = true;
            Amount = 0;
            UpdateAmountText();

            if (_isOnBoarding)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
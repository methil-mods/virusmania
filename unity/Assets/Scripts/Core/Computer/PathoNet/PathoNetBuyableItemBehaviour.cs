using Core.Interaction;
using Core.Item;
using Core.Money;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Core.Computer.PathoNet
{
    public class PathoNetBuyableItemBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemPrice;
        private Item.Item itemData;
        private PathoNetInterface _pathoInterface;

        public void Setup(Item.Item item, PathoNetInterface pathoInterface)
        {
            itemData = item;
            _pathoInterface = pathoInterface;

            if (itemImage != null)
                itemImage.sprite = item.itemIcon;

            if (itemName != null)
                itemName.text = item.itemName;

            if (itemPrice != null)
                itemPrice.text = "$" + item.price;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AddToCart();
        }

        private void AddToCart()
        {
            if (MoneyController.Instance.CanRemoveMoney(itemData.price))
                _pathoInterface.AddItemToCart(itemData);
        }
    }
}
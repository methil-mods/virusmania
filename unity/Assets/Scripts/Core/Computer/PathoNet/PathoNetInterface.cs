using System.Collections.Generic;
using Core.Interaction;
using UnityEngine;
using UnityEngine.UI;
using Core.Item;
using Core.Item.Holder;
using Core.Money;
using Core.Player;
using UnityEngine.Events;

namespace Core.Computer.PathoNet
{
    public class PathoNetInterface : MonoBehaviour
    {
        public GameObject pathoBuyableItemPrefab;
        public GameObject pathoCartItemPrefab;
        public Transform pathoBuyableItemContainer;
        public Transform pathoCartItemContainer;
        public Button buyButton;
        [SerializeField] private PathoNetItemReceiver pathoItemReceiver;
        [SerializeField] private int maxCartItems = 3;

        private Dictionary<Item.Item, GameObject> cartItems = new Dictionary<Item.Item, GameObject>();
        public UnityAction OnBuyCart;

        protected void Start()
        {
            foreach (Transform child in pathoBuyableItemContainer)
                Destroy(child.gameObject);
            foreach (Transform child in pathoCartItemContainer)
                Destroy(child.gameObject);

            foreach (var item in ItemDatabase.Instance.BuyableItems)
            {
                GameObject go = Instantiate(pathoBuyableItemPrefab, pathoBuyableItemContainer);
                var buyableBehaviour = go.GetComponent<PathoNetBuyableItemBehaviour>();
                if (buyableBehaviour != null)
                    buyableBehaviour.Setup(item, this);
            }

            buyButton.onClick.AddListener(BuyItemInCart);
        }

        public void AddItemToCart(Item.Item item)
        {
            if (pathoCartItemContainer.childCount >= maxCartItems)
                return;

            if (cartItems.ContainsKey(item))
                return;

            GameObject go = Instantiate(pathoCartItemPrefab, pathoCartItemContainer);
            var buyableBehaviour = go.GetComponent<PathoNetBuyableItemBehaviour>();
            if (buyableBehaviour != null)
                buyableBehaviour.Setup(item, this);

            cartItems[item] = go;
        }

        public void BuyItemInCart()
        {
            if (cartItems.Count == 0)
                return;

            int totalPrice = 0;
            foreach (var item in cartItems.Keys)
                totalPrice += item.price;

            if (!MoneyController.Instance.CanRemoveMoney(totalPrice))
                return;

            MoneyController.Instance.RemoveMoney(totalPrice);

            foreach (var kvp in cartItems)
            {
                var item = kvp.Key;
                var go = kvp.Value;

                pathoItemReceiver.AddItem(new HoldItem(item));

                Destroy(go);
            }

            OnBuyCart?.Invoke();
            cartItems.Clear();
        }
    }
}

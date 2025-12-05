using System.Collections.Generic;
using Core.Interaction;
using UnityEngine;
using UnityEngine.UI;
using Core.Item;
using Core.Money;
using TMPro;
using UnityEngine.Events;

namespace Core.Computer.PathoNet
{
    public class PathoNetInterface : MonoBehaviour
    {
        public GameObject pathoBuyableItemPrefab;
        public Transform pathoBuyableItemContainer;
        public Button buyButton;
        public PathoNetItemReceiver pathoItemReceiver;
        public UnityAction OnBuyCart;
        public UnityAction<Item.Item> OnBuyItem;
        
        [SerializeField] 
        private bool onlyOnBoarding;

        public TextMeshProUGUI dollarAmountText;

        protected void Start()
        {
            foreach (Transform child in pathoBuyableItemContainer)
                Destroy(child.gameObject);
            
            var database = onlyOnBoarding ? ItemDatabase.Instance.buyableOnBoardingItems : ItemDatabase.Instance.buyableItems;
            foreach (var item in database)
            {
                GameObject go = Instantiate(pathoBuyableItemPrefab, pathoBuyableItemContainer);
                var buyableBehaviour = go.GetComponent<PathoNetBuyableItemBehaviour>();
                if (buyableBehaviour != null)
                    buyableBehaviour.Setup(item, this, onlyOnBoarding);
            }
            
            buyButton.onClick.AddListener(BuyCart);
        }

        public void UpdateInterface()
        {
            int total = 0;

            foreach (Transform child in pathoBuyableItemContainer)
            {
                var buyable = child.GetComponent<PathoNetBuyableItemBehaviour>();
                if (buyable != null && buyable.itemData != null)
                    total += buyable.Amount * buyable.itemData.price;
            }

            dollarAmountText.text = "$" + total;
        }

        private void BuyCart()
        {
            int total = 0;
            List<PathoNetBuyableItemBehaviour> itemsToBuy = new List<PathoNetBuyableItemBehaviour>();

            foreach (Transform child in pathoBuyableItemContainer)
            {
                var buyable = child.GetComponent<PathoNetBuyableItemBehaviour>();
                if (buyable != null && buyable.itemData != null && buyable.Amount > 0)
                {
                    itemsToBuy.Add(buyable);
                    total += buyable.Amount * buyable.itemData.price;
                }
            }

            if (MoneyController.Instance != null)
            {
                if (!MoneyController.Instance.CanRemoveMoney(total))
                {
                    MoneyController.Instance.OnMoneyInsufficient?.Invoke();
                    return;
                }

                MoneyController.Instance.RemoveMoney(total);
            }
            else
            {
                Debug.LogWarning("MoneyController instance is null");
            }

            foreach (PathoNetBuyableItemBehaviour buyable in itemsToBuy)
            {
                buyable.Buy();
            }

            UpdateInterface();
            OnBuyCart?.Invoke();
        }

    }
}

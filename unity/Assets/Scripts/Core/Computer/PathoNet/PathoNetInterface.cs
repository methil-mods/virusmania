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
        public Transform pathoBuyableItemContainer;
        public Button buyButton;
        [SerializeField] private PathoNetItemReceiver pathoItemReceiver;
        public UnityAction OnBuyCart;

        protected void Start()
        {
            foreach (Transform child in pathoBuyableItemContainer)
                Destroy(child.gameObject);

            foreach (var item in ItemDatabase.Instance.BuyableItems)
            {
                GameObject go = Instantiate(pathoBuyableItemPrefab, pathoBuyableItemContainer);
                var buyableBehaviour = go.GetComponent<PathoNetBuyableItemBehaviour>();
                if (buyableBehaviour != null)
                    buyableBehaviour.Setup(item, this);
            }
        }
    }
}

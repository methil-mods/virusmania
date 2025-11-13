using Core.Interaction;
using UnityEngine;
using Core.Item;

namespace Core.Computer.PathoNet
{
    public class PathoNetInterface : MonoBehaviour
    {
        public GameObject pathoBuyableItemPrefab;
        public GameObject pathoCartItemPrefab;
        public Transform pathoBuyableItemContainer;
        public Transform pathoCartItemContainer;
        [SerializeField] private PathoNetItemReceiver pathoItemReceiver;
        [SerializeField] private int maxCartItems = 3;

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
        }

        public void AddItemToCart(Item.Item item)
        {
            if (pathoCartItemContainer.childCount >= maxCartItems)
            {
                Debug.Log("Cart is full. Max items: " + maxCartItems);
                return;
            }

            GameObject go = Instantiate(pathoCartItemPrefab, pathoCartItemContainer);
            var buyableBehaviour = go.GetComponent<PathoNetBuyableItemBehaviour>();
            if (buyableBehaviour != null)
                buyableBehaviour.Setup(item, this);
        }
    }
}
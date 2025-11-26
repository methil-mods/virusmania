using Core.Item.Holder;
using UnityEngine;

namespace Core.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
    public class Item : ScriptableObject
    {
        [Header("Item base property")]
        public string itemName;
        [TextArea]
        public string itemDescription;
        public GameObject itemPrefab;
        public Sprite itemIcon;
        public int price;

        public virtual HoldItem GetHoldItem()
        {
            return new HoldItem(this);
        }
    }
}
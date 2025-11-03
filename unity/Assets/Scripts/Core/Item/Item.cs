using Core.Interaction;
using UnityEngine;

namespace Core.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
    public class Item : ScriptableObject
    {
        [Header("Item base property")]
        public string itemName;
        public string itemDescription;

        public virtual HoldItem GetHoldItem()
        {
            return new HoldItem(this);
        }
    }
}
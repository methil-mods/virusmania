using Core.Item.Holder;
using UnityEngine;

namespace Core.Item
{
    [CreateAssetMenu(fileName = "VirusItem", menuName = "Item/VirusItem")]
    public class VirusItem : Item
    {
        public Sprite virusIcon;
        
        public override HoldItem GetHoldItem()
        {
            return new HoldVirusItem(this);
        }
    }
}
using UnityEngine;

namespace Core.Brief
{
    [CreateAssetMenu(menuName = "Brief/Brief", fileName = "Brief")]
    public class Brief : ScriptableObject
    {
        public Sprite briefSprite;
        public string briefTitle;
        public string briefDescription;
        public Item.Item wantedItem;

        [Min(0)]
        public int moneyGiven;
    }
}
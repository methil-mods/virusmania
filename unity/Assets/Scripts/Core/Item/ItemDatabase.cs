using System.Collections.Generic;
using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.Item
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/ItemDatabase")]
    public class ItemDatabase : SingletonScriptableDatabase<ItemDatabase, Item>
    {
        public List<Item> buyableOnBoardingItems;
        public List<Item> buyableItems;
    }
}
using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.Item
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/ItemDatabase")]
    public class ItemDatabase : SingletonScriptableDatabase<ItemDatabase, Item>
    {
        
    }
}
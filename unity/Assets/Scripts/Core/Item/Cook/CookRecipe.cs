using UnityEngine;

namespace Core.Item.Cook
{
    [CreateAssetMenu(fileName = "CookRecipe", menuName = "Item/CookRecipe")]
    public class CookRecipe : ScriptableObject
    {
        [Header("Item to cook")]
        public Item inputItem;

        [Header("Resulting item")]
        public Item resultItem;
    }
}
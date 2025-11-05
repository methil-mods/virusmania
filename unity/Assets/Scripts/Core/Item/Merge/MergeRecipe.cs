using UnityEngine;

namespace Core.Item.Merge
{
    [CreateAssetMenu(fileName = "MergeRecipe", menuName = "Item/MergeRecipe")]
    public class MergeRecipe : ScriptableObject
    {
        [Header("Items to merge")]
        public Item[] inputItems;

        [Header("Resulting item")]
        public Item resultItem;

        public bool Matches(Item[] items)
        {
            if (items.Length != inputItems.Length) return false;
            
            foreach (var input in inputItems)
            {
                bool found = false;
                foreach (var item in items)
                {
                    if (item == input)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }

            return true;
        }
    }
}
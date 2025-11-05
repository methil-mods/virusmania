using System.Linq;
using UnityEngine;

namespace Core.Item.Merge
{
    public static class MergeUtils
    {
        public static Item TryMerge(params Item[] items)
        {
            foreach (var recipe in MergeDatabase.Instance.Database)
            {
                if (recipe.Matches(items))
                {
                    Debug.Log($"Merged {string.Join(", ", items.Select(i => i.itemName))} into {recipe.resultItem.itemName}");
                    return recipe.resultItem;
                }
            }

            Debug.Log("No valid merge recipe found.");
            return null;
        }
        
    }
}
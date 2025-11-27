using Core.Item.Holder;
using UnityEngine;

namespace Core.Item.Merge
{
    [CreateAssetMenu(fileName = "MergeRecipe", menuName = "Item/MergeRecipe")]
    public class MergeRecipe<TInput, TResult> : MergeRecipeBase where TInput: Item where TResult: Item
    {
        [Header("Items to merge")]
        public TInput[] inputItems;

        [Header("Resulting item")]
        public TResult resultItem;

        public override bool Matches(Item[] items)
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

        public override HoldItem GetResultItem()
        {
            return resultItem.GetHoldItem();
        }
    }
}
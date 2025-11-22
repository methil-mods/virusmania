using Core.Item.Holder;
using UnityEngine;

namespace Core.Item.Merge
{
    [CreateAssetMenu(fileName = "MergeVirusRecipe", menuName = "MergeRecipe/MergeVirusRecipe")]
    public class MergeVirusRecipe : MergeRecipe<MicrobeItem, VirusItem>
    {
        public override HoldItem GetResultItem()
        {
            HoldItem holdVirusItem = resultItem.GetHoldItem();
            return holdVirusItem;
        }
    }
}
using Core.Item.Holder;
using UnityEngine;

namespace Core.Item.Merge
{
    public abstract class MergeRecipeBase : ScriptableObject
    {
        public bool isOnBoarding = false;
        
        public abstract bool Matches(Item[] items);
        public abstract HoldItem GetResultItem();
    }
}
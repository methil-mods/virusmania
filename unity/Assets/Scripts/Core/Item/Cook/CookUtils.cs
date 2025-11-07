using System.Linq;
using UnityEngine;

namespace Core.Item.Cook
{
    public static class CookUtils
    {
        public static Item TryCook(Item inputItem)
        {
            if (inputItem == null)
                return null;
            
            CookRecipe recipe = CookDatabase.Instance.Database.FirstOrDefault(r => r.inputItem != null && r.inputItem == inputItem);

            if (recipe != null && recipe.resultItem != null)
                return recipe.resultItem;

            return null;
        }
    }
}
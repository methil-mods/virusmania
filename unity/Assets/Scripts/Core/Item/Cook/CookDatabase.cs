using Core.Item.Cook;
using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.Item.Cook
{
    [CreateAssetMenu(fileName = "CookDatabase", menuName = "Item/CookDatabase")]
    public class CookDatabase : SingletonScriptableDatabase<CookDatabase, CookRecipe>
    {
        
    }
}
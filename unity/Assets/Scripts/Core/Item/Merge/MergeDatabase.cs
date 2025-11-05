using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.Item.Merge
{
    [CreateAssetMenu(fileName = "MergeDatabase", menuName = "Item/MergeDatabase")]
    public class MergeDatabase : SingletonScriptableDatabase<MergeDatabase, MergeRecipe>
    {
        
    }
}
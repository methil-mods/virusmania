using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Framework.ScriptableObjects
{
    public class SingletonScriptableDatabase<T, TData> : SingletonScriptableObject<T> 
        where T : SingletonScriptableObject<T> where TData : ScriptableObject
    {
        [SerializeField]
        public List<TData> Database;

        public TData GetRandom()
        {
            if (Database == null || Database.Count == 0) return default;
            return Database[Random.Range(0, Database.Count)];
        }

#if UNITY_EDITOR
        public void GetAllData()
        {
            Database = new List<TData>();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(TData).Name}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TData asset = AssetDatabase.LoadAssetAtPath<TData>(path);
                if (asset != null && !Database.Contains(asset)) Database.Add(asset);
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
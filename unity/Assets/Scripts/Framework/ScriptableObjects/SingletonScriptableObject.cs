using UnityEngine;

namespace Framework.ScriptableObjects
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var assets = Resources.LoadAll<T>("");
                    if (assets.Length > 0) _instance = assets[0];
                }
                return _instance;
            }
        }
    }

}
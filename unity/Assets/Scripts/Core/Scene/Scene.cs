using UnityEngine;

namespace Core.Scene
{
    [CreateAssetMenu(menuName = "MtScene/Scene", fileName = "Scene")]
    public class Scene : ScriptableObject
    {
        public int sceneKey;
        public string sceneName;
    }
}
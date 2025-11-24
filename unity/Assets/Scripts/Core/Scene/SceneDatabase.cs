using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.Scene
{
    [CreateAssetMenu(menuName = "MtScene/SceneDatabase", fileName = "SceneDatabase")]
    public class SceneDatabase : SingletonScriptableDatabase<SceneDatabase, Scene>
    {
        public Scene GetSceneByName(string sceneName)
        {
            return this.Database.Find(scene => scene.sceneName == sceneName);
        }
    }
}
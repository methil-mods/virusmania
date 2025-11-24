using UnityEngine;

namespace Core.Scene
{
    public class SceneTransitionButton : MonoBehaviour
    {
        public string sceneName;
        
        public void GoOnNewScene()
        {
            var newScene = SceneDatabase.Instance.GetSceneByName(sceneName);
            if(newScene != null)
                SceneTransitor.Instance.LoadScene(newScene);
            else 
                Debug.LogError($"Scene not found in database: {sceneName}");
        }
    }
}
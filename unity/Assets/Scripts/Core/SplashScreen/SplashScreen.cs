using System;
using System.Collections;
using Core.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SplashScreen
{
    public class SplashScreen : MonoBehaviour
    {
        public CanvasGroup parentCanvasGroup;

        public void Start()
        {
            DontDestroyOnLoad(parentCanvasGroup.gameObject);
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadMainMenuCoroutine());
        }

        public IEnumerator LoadMainMenuCoroutine()
        {
            yield return new WaitForEndOfFrame();
            var mainMenu = SceneDatabase.Instance.GetSceneByName("MainMenu");
            
            AsyncOperation asyncSceneToLoad = SceneManager.LoadSceneAsync(mainMenu.sceneKey);
            asyncSceneToLoad.allowSceneActivation = false; // stop the level from activating
            while (asyncSceneToLoad.progress < 0.9f)
            {
                yield return new WaitForEndOfFrame();
            } 
            asyncSceneToLoad.allowSceneActivation = true; // this will enter the level now
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();
            
            LeanTween.alphaCanvas(parentCanvasGroup, 0f, 1f)
                .setEase( LeanTweenType.easeOutQuart )
                .setIgnoreTimeScale(true);
            
            yield return new WaitForSeconds(1f);
            Destroy(parentCanvasGroup.gameObject);
        }
    }
}
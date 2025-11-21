using Core.Scene;
using Framework.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MainMenu
{
    public class MainMenuController : BaseController<MainMenuController>
    {
        public Image maskImage;
        
        public void QuitApplication()
        {
            Application.Quit();
        }
        
        public int newScene;
        
        public void GoOnNewScene()
        {
            /*
            LeanTween.moveLocal(maskImage.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f), 0.3f)
                .setEaseSpring();
               LeanTween.delayedCall(0.95f, (_ => { SceneTransitor.Instance.LoadScene(newScene); }));
               LeanTween.size(maskImage.GetComponent<RectTransform>(), Vector3.zero, 0.8f)
                   .setEaseSpring();
                */
            SceneTransitor.Instance.LoadScene(newScene);
        }
    }
}
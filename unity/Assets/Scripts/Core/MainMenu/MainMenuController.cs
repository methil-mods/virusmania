using System;
using Core.Scene;
using Framework.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MainMenu
{
    public class MainMenuController : BaseController<MainMenuController>
    {
        public RawImage maskImage;
        public RawImage maskImage2;
        public RawImage maskImage3;
        public RawImage maskImage4;
        public RawImage maskImage5;

        public void Start()
        {
            var material = new Material(maskImage.material);
            maskImage.material = material;
            maskImage2.material = material;
            maskImage3.material = material;
            maskImage4.material = material;
            maskImage5.material = material;
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
        
        public int newScene;
        
        public void GoOnNewScene()
        {
            LeanTween.value(maskImage.gameObject, (float value) => {
                maskImage.material.SetFloat("_MaskSize", value);
            }, maskImage.material.GetFloat("_MaskSize"), 0f, 0.6f).setEaseSpring();

            LeanTween.value(maskImage.gameObject, (Vector3 value) => {
                maskImage.material.SetVector("_MaskOffset", value);
            }, maskImage.material.GetVector("_MaskOffset"), new Vector3(0.2f, 0.5f, 0f), 0.6f)
                .setEaseSpring();

            LeanTween.delayedCall(0.4f, () => { SceneTransitor.Instance.LoadScene(newScene); });
        }
    }
}
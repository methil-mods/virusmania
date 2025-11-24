using System;
using Core.Scene;
using Framework.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MainMenu
{
    public class MainMenuController : BaseController<MainMenuController>
    {
        [SerializeField] private RawImage[] maskImages;
        
        [NonSerialized]
        public Material sharedMaterial;

        private void Start()
        {
            sharedMaterial = new Material(maskImages[0].material);
            for (int i = 0; i < maskImages.Length; i++)
                maskImages[i].material = sharedMaterial;
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void ShowOnBoardingChoice()
        {
            OnBoardingMainMenuController.Instance.OpenPanel();
        }
    }
}
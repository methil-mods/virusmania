using System;
using Core.Scene;
using Framework.Controller;
using UnityEngine;

namespace Core.OnBoarding
{
    public class OnBoardingController : BaseController<OnBoardingController>
    {
        public int actualOnBoardingIndex = 0;

        public OnBoardingData GetActualOnBoarding()
        {
            return OnBoardingDatabase.Instance.OnBoardings[actualOnBoardingIndex];
        }

        public void Start()
        {
            OnBoardingInterface.Instance.ShowActualBoard();
        }

        public void FinishOnBoarding()
        {
            var newScene = SceneDatabase.Instance.GetSceneByName("Game");
            SceneTransitor.Instance.LoadScene(newScene);
        }
    }
}
using System;
using System.Collections.Generic;
using Framework.Controller;
using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.OnBoarding
{
    [CreateAssetMenu(fileName = "OnBoardingDatabase", menuName = "OnBoarding/OnBoardingDatabase")]
    public class OnBoardingDatabase : SingletonScriptableObject<OnBoardingDatabase>
    {
        [SerializeField]
        public List<OnBoardingData> OnBoardings = new List<OnBoardingData>();
    }

    [Serializable]
    public class OnBoardingData
    {
        public string onBoardingName;
        public string onBoardingDescription;
        public OnBoardingState onBoardingState;
    }

    public enum OnBoardingState
    {
        BuyItems
    }
}
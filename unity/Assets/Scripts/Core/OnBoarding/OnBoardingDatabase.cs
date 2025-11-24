using System.Collections.Generic;
using Framework.Controller;
using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.OnBoarding
{
    [CreateAssetMenu(fileName = "OnBoardingDatabase", menuName = "OnBoarding/OnBoardingDatabase")]
    public class OnBoardingDatabase : SingletonScriptableObject<OnBoardingDatabase>
    {
        public List<OnBoardingData> OnBoardings = new List<OnBoardingData>();
    }

    public class OnBoardingData
    {
        public string OnBoardingName;
        public string OnBoardingDescription;
    }
}
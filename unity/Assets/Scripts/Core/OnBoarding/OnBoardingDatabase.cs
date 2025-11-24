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

        public Item.Item itemToBuy1;
        public Item.Item itemToBuy2;
        
        public Item.Item itemToCook;
        
        public Item.Item itemToMerge;
        
        public Item.Item itemToSent;
    }

    [Serializable]
    public class OnBoardingData
    {
        public string onBoardingName;
        public string onBoardingDescription;
        [TextArea]
        public string onBoardingHint;
        public OnBoardingState onBoardingState;
    }

    public enum OnBoardingState
    {
        BuyItems,
        CookItem,
        MergeItems,
        SendItem
    }
}
using System;
using System.Linq;
using Core.Computer.PathoNet;
using Core.Interaction;
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

        [SerializeField]
        private BrewInteractable brewInteractable;
        [SerializeField]
        private CookInteractable cookInteractable;
        [SerializeField]
        private PathoNetInterface pathoNetInterface;
        [SerializeField]
        private SendItemInteractable sendItemInteractable;

        private int _itemToBuy1Count = 0;
        private int _itemToBuy2Count = 0;
        
        public void Start()
        {
            OnBoardingInterface.Instance.ShowActualBoard();

            sendItemInteractable.onItemSent += item =>
            {
                OnBoardingData boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.SendItem)
                {
                    if (item == OnBoardingDatabase.Instance.itemToSent)
                        GoNextOnBoarding();
                }
            };

            brewInteractable.onItemMerged += item =>
            {
                OnBoardingData boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.MergeItems)
                {
                    if (item == OnBoardingDatabase.Instance.itemToMerge)
                        GoNextOnBoarding();
                }
            };

            cookInteractable.onItemCooked += item =>
            {
                OnBoardingData boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.CookItem)
                {
                    if (item == OnBoardingDatabase.Instance.itemToCook)
                        GoNextOnBoarding();
                }
            };

            pathoNetInterface.OnBuyItem += item =>
            {
                OnBoardingData boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.BuyItems)
                {
                    if (_itemToBuy1Count == 1 && _itemToBuy2Count == 1)
                        return;
                    
                    if (item == OnBoardingDatabase.Instance.itemToBuy1)
                        _itemToBuy1Count++;
                    
                    if (item == OnBoardingDatabase.Instance.itemToBuy2)
                        _itemToBuy2Count++;
                    
                    if (_itemToBuy1Count == 1 && _itemToBuy2Count == 1)
                        GoNextOnBoarding();
                }
            };
        }

        public void GoNextOnBoarding()
        {
            if (actualOnBoardingIndex >= OnBoardingDatabase.Instance.OnBoardings.Count) return;
            actualOnBoardingIndex++;
            if(actualOnBoardingIndex >= OnBoardingDatabase.Instance.OnBoardings.Count)
            {
                FinishOnBoarding();
                return;
            }
            OnBoardingInterface.Instance.ShowActualBoard();
        }

        public void FinishOnBoarding()
        {
            var newScene = SceneDatabase.Instance.GetSceneByName("Game");
            SceneTransitor.Instance.LoadScene(newScene);
        }
    }
}
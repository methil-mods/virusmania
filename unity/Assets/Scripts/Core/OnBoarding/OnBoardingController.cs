using System;
using System.Linq;
using Core.Computer.PathoNet;
using Core.Interaction;
using Core.MergeLibrary;
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
        [SerializeField]
        private RecipeListInteractable recipeListInteractable;

        [SerializeField] private GameObject brewInteractableObject;
        [SerializeField] private GameObject cookInteractableObject;
        [SerializeField] private GameObject computerObject;
        [SerializeField] private GameObject cartObject;
        [SerializeField] private GameObject sendItemInteractableObject;

        private int _itemToBuy1Count = 0;
        private int _itemToBuy2Count = 0;

        private void Pop(GameObject obj)
        {
            obj.SetActive(true);
            obj.transform.localScale = Vector3.zero;
            LeanTween.scale(obj, Vector3.one, 0.35f).setEaseSpring();
        }

        public void Start()
        {
            computerObject.SetActive(false);
            cookInteractableObject.SetActive(false);
            sendItemInteractableObject.SetActive(false);
            cartObject.SetActive(false);
            brewInteractableObject.SetActive(false);
            computerObject.transform.localScale = Vector3.zero;
            cookInteractableObject.transform.localScale = Vector3.zero;
            sendItemInteractableObject.transform.localScale = Vector3.zero;
            cartObject.transform.localScale = Vector3.zero;
            brewInteractableObject.transform.localScale = Vector3.zero;

            OnBoardingInterface.Instance.ShowActualBoard();

            recipeListInteractable.OnInteractRecipeList += () =>
            {
                var boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.OpenLibrary)
                {
                    Pop(computerObject);
                    Pop(cartObject);
                    GoNextOnBoarding();
                }
            };

            sendItemInteractable.onItemSent += item =>
            {
                var boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.SendItem)
                {
                    if (item == OnBoardingDatabase.Instance.itemToSent)
                        GoNextOnBoarding();
                }
            };

            brewInteractable.onItemMerged += item =>
            {
                var boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.MergeItems)
                {
                    if (item == OnBoardingDatabase.Instance.itemToMerge)
                    {
                        Pop(sendItemInteractableObject);
                        GoNextOnBoarding();
                    }
                }
            };

            cookInteractable.onItemCooked += item =>
            {
                var boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.CookItem)
                {
                    if (item == OnBoardingDatabase.Instance.itemToCook)
                    {
                        Pop(brewInteractableObject);
                        GoNextOnBoarding();
                    }
                }
            };

            pathoNetInterface.OnBuyItem += item =>
            {
                var boardingData = GetActualOnBoarding();
                if (boardingData.onBoardingState == OnBoardingState.BuyItems)
                {
                    if (_itemToBuy1Count == 1 && _itemToBuy2Count == 1)
                        return;

                    if (item == OnBoardingDatabase.Instance.itemToBuy1)
                        _itemToBuy1Count++;

                    if (item == OnBoardingDatabase.Instance.itemToBuy2)
                        _itemToBuy2Count++;

                    if (_itemToBuy1Count == 1 && _itemToBuy2Count == 1)
                    {
                        GoNextOnBoarding();
                        Pop(cookInteractableObject);
                    }
                }
            };
        }

        public void GoNextOnBoarding()
        {
            if (actualOnBoardingIndex >= OnBoardingDatabase.Instance.OnBoardings.Count) return;
            actualOnBoardingIndex++;
            if (actualOnBoardingIndex >= OnBoardingDatabase.Instance.OnBoardings.Count)
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

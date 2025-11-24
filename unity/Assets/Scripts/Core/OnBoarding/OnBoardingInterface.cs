using Framework.Controller;
using TMPro;
using UnityEngine;

namespace Core.OnBoarding
{
    public class OnBoardingInterface : BaseController<OnBoardingInterface>
    {
        public TextMeshProUGUI onBoardingDescriptionText;
        
        public void ShowActualBoard()
        {
            OnBoardingData data = OnBoardingController.Instance.GetActualOnBoarding();
            
            onBoardingDescriptionText.text = data.onBoardingDescription;
        }
    }
}
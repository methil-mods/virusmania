using System;
using Core.Timer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Brief
{
    public class BriefInterface : MonoBehaviour
    {
        [Header("Actual Brief Panel References")]
        public RectTransform actualBriefPanel;
        public TextMeshProUGUI actualBriefTitle;
        public TextMeshProUGUI actualBriefDescription;
        public TextMeshProUGUI actualBriefMoneyGiven;
        
        [Header("New Brief Panel References")]
        public RectTransform briefPanel;
        public TextMeshProUGUI briefNameText;
        public TextMeshProUGUI briefDescriptionText;
        public TextMeshProUGUI briefMoneyGivenText;
        public Button briefEndButton;

        private Brief _tempNewBrief;

        public void Start()
        {
            briefEndButton.onClick.AddListener(PutNewBrief);
        }

        public void Update()
        {
            if (BriefController.Instance.actualBrief == null)
            {
                actualBriefPanel.gameObject.SetActive(false);
            }
            else
            {
                actualBriefPanel.gameObject.SetActive(true);
                actualBriefTitle.text = BriefController.Instance.actualBrief.briefTitle;
                actualBriefDescription.text = BriefController.Instance.actualBrief.briefDescription;
                actualBriefMoneyGiven.text = $"{BriefController.Instance.actualBrief.moneyGiven} $";
            }
        }

        public void SetupNewBriefShow(Brief brief)
        {
            briefPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
            briefPanel.gameObject.SetActive(true);
            
            LeanTween.scale(briefPanel.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), .4f)
                .setEase(LeanTweenType.easeSpring);
            briefNameText.text = brief.briefTitle;
            briefDescriptionText.text = brief.briefDescription;
            briefMoneyGivenText.text = $"{brief.moneyGiven} $";
            
            _tempNewBrief = brief;
        }

        public void PutNewBrief()
        {
            BriefController.Instance.actualBrief = _tempNewBrief;
            TimerController.Instance.LaunchTimer(_tempNewBrief.timeForBrief, (() =>
            {
                Debug.LogWarning("NEED TO SET LOOSE");
                // TODO : Logics to loose
            }));
            _tempNewBrief = null;
            
            HideBriefPanel();
        }
        
        public void HideBriefPanel()
        {
            LeanTween.scale(briefPanel.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f), .4f)
                .setEase(LeanTweenType.easeOutCirc)
                .setOnComplete((() =>
                {
                    briefPanel.gameObject.SetActive(false);
                }));
        }
    }
}
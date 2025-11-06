using System;
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
        
        [Header("New Brief Panel References")]
        public RectTransform briefPanel;
        public TextMeshProUGUI briefNameText;
        public TextMeshProUGUI briefDescriptionText;
        public Button briefEndButton;

        public void Start()
        {
            briefPanel.gameObject.SetActive(false);
            briefEndButton.onClick.AddListener(HideBriefPanel);
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
            }
        }

        public void SetupNewBriefShow(Brief brief)
        {
            briefPanel.gameObject.SetActive(true);
            briefNameText.text = brief.briefTitle;
            briefDescriptionText.text = brief.briefDescription;
        }

        public void HideBriefPanel()
        {
            briefPanel.gameObject.SetActive(false);
        }
    }
}
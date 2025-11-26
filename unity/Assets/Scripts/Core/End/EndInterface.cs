using System;
using Core.Scene;
using Core.Timer;
using Framework.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.End
{
    public class EndInterface : BaseController<EndInterface>
    {
        public Image blackPanel;
        
        public RectTransform openWinPanel;
        public RectTransform openLosePanel;

        public void Start()
        {
            blackPanel.color = new Color(0, 0, 0, 0);
            openLosePanel.gameObject.SetActive(false);
            openLosePanel.gameObject.SetActive(false);
            
            TimerController.Instance.OnTimerEnd += OpenLosePanel;
        }

        public void OnDisable()
        {
            TimerController.Instance.OnTimerEnd -= OpenLosePanel;
        }

        public void OpenWinPanel()
        {
            LeanTween.color(blackPanel.GetComponent<RectTransform>(), new Color(0, 0, 0, 0.6f), 0.6f)
                .setEaseOutCirc();;
            openWinPanel.gameObject.SetActive(true);
        }

        public void OpenLosePanel()
        {
            LeanTween.color(blackPanel.GetComponent<RectTransform>(), new Color(0, 0, 0, 0.6f), 0.6f)
                .setEaseOutCirc();;
            openLosePanel.gameObject.SetActive(true);
        }

        public void ReturnToMainMenu()
        {
            var scene = SceneDatabase.Instance.GetSceneByName("MainMenu");
            SceneTransitor.Instance.LoadScene(scene);
        }
    }
}
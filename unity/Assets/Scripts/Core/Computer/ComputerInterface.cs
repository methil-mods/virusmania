using System;
using System.Collections.Generic;
using Core.Input;
using Core.PostProcess;
using UnityEngine;
using UnityEngine.UI;
using Framework.Controller;

namespace Core.Computer
{
    public class ComputerInterface : InterfaceController<ComputerInterface>
    {
        public void Start()
        {
            base.Start();
        }
        
        public override void OpenPanel()
        {
            if (!CanOpen() || panel == null) return;
            
            panel.GetComponent<RectTransform>().localScale = Vector3.zero;
            
            PostProcessController.Instance.OnShowPanelPostProcess();
            InputDatabase.Instance.moveAction.action.Disable();
            
            LeanTween.cancel(panel);
            LeanTween.scale(panel.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), .4f)
                .setEase(LeanTweenType.easeSpring);
            
            OnPanelOpen?.Invoke();
            panel.SetActive(true);
        }

        public override void ClosePanel()
        {
            if (!PanelIsActive()) return;
            if (panel == null) return;
            
            PostProcessController.Instance.OnHidePanelPostProcess();
            InputDatabase.Instance.moveAction.action.Enable();
            
            LeanTween.cancel(panel);
            LeanTween.scale(panel.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f), .4f)
                .setEase(LeanTweenType.easeOutCirc)
                .setOnComplete((() =>
                {
                    panel.gameObject.SetActive(false);
                }));
            
            OnPanelClose?.Invoke();
        }
    }
}
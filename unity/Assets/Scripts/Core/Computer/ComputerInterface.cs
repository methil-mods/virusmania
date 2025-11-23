using System;
using System.Collections.Generic;
using Core.Computer.PathoNet;
using Core.Input;
using Core.PostProcess;
using UnityEngine;
using UnityEngine.UI;
using Framework.Controller;

namespace Core.Computer
{
    public class ComputerInterface : InterfaceController<ComputerInterface>
    {
        public override void Start()
        {
            base.Start();

            panel.GetComponentInChildren<PathoNetInterface>().OnBuyCart += ClosePanel;
        }
        
        public override void OpenPanel()
        {
            if (!CanOpen() || panel == null) return;
            
            panel.GetComponent<RectTransform>().localScale = Vector3.zero;
            
            if (PostProcessController.Instance != null) 
                PostProcessController.Instance.OnShowPanelPostProcess();
            InputDatabase.Instance.DisableInputs();
            
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
            
            if (PostProcessController.Instance != null) 
                PostProcessController.Instance.OnHidePanelPostProcess();
            InputDatabase.Instance.EnableInputs();
            
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
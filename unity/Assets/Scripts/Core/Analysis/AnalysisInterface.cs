using Core.Input;
using Core.Item.Holder;
using Core.PostProcess;
using Framework.Controller;
using UnityEngine;

namespace Core.Analysis
{
    public class AnalysisInterface : InterfaceController<AnalysisInterface>
    {
        private HoldItem _holdItem;
        
        public void ShowAnalysis(HoldItem holdItemAnalyzed)
        {
            _holdItem = holdItemAnalyzed;
            OpenPanel();
        }
        
        public override void OpenPanel()
        {
            if (!CanOpen() || panel == null) return;
            
            panel.GetComponent<RectTransform>().localScale = Vector3.zero;
            
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
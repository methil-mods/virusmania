using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Controller
{
    public class InterfaceController<T> : BaseController<T> where T : InterfaceController<T>
    {
        [SerializeField] protected GameObject panel;
        [SerializeField] protected Image blackPanel;

        public UnityAction OnPanelOpen;
        public UnityAction OnPanelClose;

        public virtual void Start()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            if (blackPanel != null)
            {
                blackPanel.color = new Color(0, 0, 0, 0);
            }
        }

        public virtual bool CanOpen() => true;

        public virtual void OpenPanel()
        {
            if (!CanOpen() || panel == null) return;

            if (blackPanel != null)
            {
                LeanTween.cancel(blackPanel.gameObject);
                LeanTween.color(blackPanel.GetComponent<RectTransform>(), new Color(0, 0, 0, 0.6f), 0.6f)
                    .setEaseOutCirc();;
            }
            OnPanelOpen?.Invoke();
            panel.SetActive(true);
        }

        public virtual void ClosePanel()
        {
            if (!PanelIsActive()) return;
            if (panel == null) return;
            

            if (blackPanel != null)
            {
                LeanTween.cancel(blackPanel.gameObject);
                LeanTween.color(blackPanel.GetComponent<RectTransform>(), new Color(0, 0, 0, 0), 0.6f)
                    .setEaseOutCirc();;
            }
            OnPanelClose?.Invoke();
            panel.SetActive(false);
        }

        protected bool PanelIsActive()
        {
            return panel.activeSelf;
        }
    }
}

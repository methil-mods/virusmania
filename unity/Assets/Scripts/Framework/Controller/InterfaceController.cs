using UnityEngine;
using UnityEngine.Events;

namespace Framework.Controller
{
    public class InterfaceController<T> : BaseController<T> where T : InterfaceController<T>
    {
        [SerializeField] protected GameObject panel;

        public UnityAction OnPanelOpen;
        public UnityAction OnPanelClose;

        protected void Start()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        public virtual bool CanOpen() => true;

        public virtual void OpenPanel()
        {
            if (!CanOpen() || panel == null) return;
            OnPanelOpen?.Invoke();
            panel.SetActive(true);
        }

        public virtual void ClosePanel()
        {
            if (!PanelIsActive()) return;
            if (panel == null) return;
            
            OnPanelClose?.Invoke();
            panel.SetActive(false);
        }

        protected bool PanelIsActive()
        {
            return panel.activeSelf;
        }
    }
}

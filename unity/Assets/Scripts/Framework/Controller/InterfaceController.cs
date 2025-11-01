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

        public void OpenPanel()
        {
            if (!CanOpen() || panel == null) return;
            panel.SetActive(true);
            OnPanelOpen?.Invoke();
        }

        public void ClosePanel()
        {
            if (!PanelIsActive()) return;
            if (panel == null)
            {
                OnPanelClose?.Invoke();
                return;
            }
            
            OnPanelClose?.Invoke();
            panel.SetActive(false);
        }

        private bool PanelIsActive()
        {
            return panel.activeSelf;
        }
    }
}

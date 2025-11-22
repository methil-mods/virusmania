using Framework.Controller;
using UnityEngine;

namespace Core.MainMenu
{
    public class OptionsController : BaseController<OptionsController>
    {
        public RectTransform optionPanel;
        public float duration = 0.25f;

        Vector3 startPos;
        Vector3 offscreenPos;
        bool initialized;

        void Init()
        {
            if (initialized) return;
            initialized = true;
            startPos = optionPanel.localPosition;
            offscreenPos = startPos + new Vector3(optionPanel.rect.width, 0, 0);
            optionPanel.localPosition = offscreenPos;
            optionPanel.gameObject.SetActive(false);
        }

        public void OpenOption()
        {
            Init();
            optionPanel.gameObject.SetActive(true);
            LeanTween.moveLocal(optionPanel.gameObject, startPos, duration)
                .setEaseSpring();
        }

        public void CloseOption()
        {
            Init();
            LeanTween.moveLocal(optionPanel.gameObject, offscreenPos, duration)
                .setEaseSpring()
                .setOnComplete(() => optionPanel.gameObject.SetActive(false));
        }
    }
}
using Framework.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MainMenu
{
    public class OptionsController : BaseController<OptionsController>
    {
        public RectTransform optionPanel;
        public float duration = 0.25f;

        public Slider musicSlider;
        public Slider interactionSlider;
        public Slider uiSlider;

        Vector3 startPos;
        Vector3 offscreenPos;
        bool initialized;

        void Init()
        {
            if (initialized) return;
            initialized = true;

            musicSlider.maxValue = 100f;
            interactionSlider.maxValue = 100f;
            uiSlider.maxValue = 100f;

            musicSlider.value = SFXDatabase.Instance.musicVolume;
            interactionSlider.value = SFXDatabase.Instance.interactionVolume;
            uiSlider.value = SFXDatabase.Instance.uiVolume;

            musicSlider.onValueChanged.AddListener(v => SFXDatabase.Instance.MusicVolume = v);
            interactionSlider.onValueChanged.AddListener(v => SFXDatabase.Instance.InteractionVolume = v);
            uiSlider.onValueChanged.AddListener(v => SFXDatabase.Instance.UserInterfaceVolume = v);

            startPos = optionPanel.localPosition;
            offscreenPos = startPos + new Vector3(optionPanel.rect.width, 0, 0);
            optionPanel.localPosition = offscreenPos;
            optionPanel.gameObject.SetActive(false);
        }

        public void OpenOption()
        {
            Init();
            optionPanel.gameObject.SetActive(true);
            if (MainMenuController.Instance != null)
                LeanTween.scale(MainMenuController.Instance.gameObject, new Vector3(0f, 0f, 0f), duration).setEaseSpring();
            LeanTween.moveLocal(optionPanel.gameObject, startPos, duration).setEaseSpring();
        }

        public void CloseOption()
        {
            Init();
            if (MainMenuController.Instance != null)
                LeanTween.scale(MainMenuController.Instance.gameObject, new Vector3(1f, 1f, 1f), duration).setEaseSpring();
            LeanTween.moveLocal(optionPanel.gameObject, offscreenPos, duration)
                .setEaseOutCirc()
                .setOnComplete(() => optionPanel.gameObject.SetActive(false));
        }
    }
}

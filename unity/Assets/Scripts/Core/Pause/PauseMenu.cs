using Core.Input;
using Core.PostProcess;
using Core.Scene;
using Framework.Controller;
using UnityEngine;
using TMPro;

namespace Core.Pause
{
    public class PauseMenu : InterfaceController<PauseMenu>
    {
        public override void Start()
        {
            base.Start();
            InputDatabase.Instance.pauseAction.action.performed += context => CallPause();
        }

        public void CallPause()
        {
            if (this.panel.activeSelf)
            {
                InputDatabase.Instance.EnableInputs();
                ClosePanel();
            }
            else
            {
                InputDatabase.Instance.DisableInputs();
                OpenPanel();
            }
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

        public void LoadMainMenu()
        {
            var sceneName = "MainMenu";
            var newScene = SceneDatabase.Instance.GetSceneByName(sceneName);
            if (newScene != null)
                SceneTransitor.Instance.LoadScene(newScene);
            else
                Debug.LogError("Scene not found in database : " + sceneName);
        }

#if UNITY_EDITOR
        [Header("Editor Only - Font Settings")]
        [SerializeField] private Font editorFont;
        [SerializeField] private TextMeshProUGUI fontNameText;

        /// <summary>
        /// Applique la font assignée dans editorFont à tous les composants TextMeshProUGUI enfants du panel
        /// </summary>
        [ContextMenu("Apply Font to All Texts")]
        public void ApplyFontToAllTexts()
        {
            if (panel == null)
            {
                Debug.LogWarning("Panel is null. Cannot apply font.");
                return;
            }

            if (editorFont == null)
            {
                Debug.LogWarning("Editor Font is null. Please assign a Font in the Inspector.");
                return;
            }

            // Convertit la Font normale en TMP_FontAsset
            TMP_FontAsset tmpFont = TMP_FontAsset.CreateFontAsset(editorFont);
            
            if (tmpFont == null)
            {
                Debug.LogError("Failed to create TMP_FontAsset from Font.");
                return;
            }

            // Met à jour le texte avec le nom de la font si fontNameText est assigné
            if (fontNameText != null)
            {
                fontNameText.text = editorFont.name;
            }

            // Récupère tous les TextMeshProUGUI dans les enfants du panel
            TextMeshProUGUI[] textComponents = panel.GetComponentsInChildren<TextMeshProUGUI>(true);

            if (textComponents.Length == 0)
            {
                Debug.LogWarning("No TextMeshProUGUI components found in panel children.");
                return;
            }

            int count = 0;
            foreach (TextMeshProUGUI textComponent in textComponents)
            {
                textComponent.font = tmpFont;
                
                // Active l'underlay
                textComponent.fontSharedMaterial.EnableKeyword("UNDERLAY_ON");
                
                // Configure les paramètres de l'underlay
                textComponent.fontSharedMaterial.SetFloat("_UnderlayOffsetX", 0.6f);
                textComponent.fontSharedMaterial.SetFloat("_UnderlayOffsetY", -0.6f);
                
                // Définit la couleur de l'underlay (RGB: 0, 250, 18, Alpha: 255)
                Color underlayColor = new Color(0f / 255f, 250f / 204f, 18f / 255f, 1f);
                textComponent.fontSharedMaterial.SetColor("_UnderlayColor", underlayColor);
                
                count++;
            }

            Debug.Log($"Font '{editorFont.name}' converted to SDF and applied with underlay settings to {count} TextMeshProUGUI component(s).");
        }
#endif
    }
}
using Framework.Controller;
using UnityEngine;

namespace Core.MainMenu
{
    public class OptionsController : BaseController<OptionsController>
    {
        public RectTransform optionPanel;

        public void OpenOption()
        {
            optionPanel.gameObject.SetActive(true);
        }

        public void CloseOption()
        {
            optionPanel.gameObject.SetActive(false);
        }
    }
}
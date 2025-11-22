using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.SFX.UI
{
    public class UIButtonHoverSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            SFXController.Instance.PlayUI(SFXDatabase.Instance.popUiClip);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            SFXController.Instance.PlayUI(SFXDatabase.Instance.clickUiClip);
        }
    }
}
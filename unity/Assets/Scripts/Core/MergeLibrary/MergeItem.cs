using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Core.MergeLibrary
{
    public class MergeItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject hoverObject;
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemText;
        public bool showItemName = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(showItemName == false) hoverObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(showItemName == false) hoverObject.SetActive(false);
        }

        public void SetupItem(Item.Item item, bool dontShowItemName = false)
        {
            if(dontShowItemName) hoverObject.SetActive(true);
            else  hoverObject.SetActive(false);
            showItemName = dontShowItemName;
            if (item == null) return;
            if (item.itemIcon != null)
                itemImage.sprite = item.itemIcon;
            itemText.text = item.itemName;
            itemText.maskable = true;
        }
    }
}
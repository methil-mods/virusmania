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

        void Start()
        {
            hoverObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverObject.SetActive(false);
        }

        public void SetupItem(Item.Item item)
        {
            if (item == null) return;
            if (item.itemIcon != null)
                itemImage.sprite = item.itemIcon;
            itemText.text = item.itemName;
            itemText.maskable = false;
        }
    }
}
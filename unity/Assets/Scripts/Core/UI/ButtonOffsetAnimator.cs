using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class ButtonOffsetAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Material _targetMaterial;
        private Color _originalColor;
        public float duration = 0.2f;
        public LeanTweenType easeType = LeanTweenType.easeOutCubic;
        public float pressDarken = 0.7f;

        private float _originalOffsetX;
        private float _originalOffsetY;

        void Start()
        {
            RawImage img = GetComponent<RawImage>();
            if (img != null)
            {
                img.material = new Material(img.material);
                _targetMaterial = img.material;
                _originalColor = img.color;
            }
            else
            {
                Image normalImage = GetComponent<Image>();
                normalImage.material = new Material(normalImage.material);
                _targetMaterial = normalImage.material;
                _originalColor = normalImage.color;
            }

            _originalOffsetX = _targetMaterial.GetFloat("_BorderOffsetX");
            _originalOffsetY = _targetMaterial.GetFloat("_BorderOffsetY");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AnimateOffsets(0f, 0f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AnimateOffsets(_originalOffsetX, _originalOffsetY);
        }

        private void AnimateOffsets(float targetX, float targetY)
        {
            float currentX = _targetMaterial.GetFloat("_BorderOffsetX");
            LeanTween.value(gameObject, currentX, targetX, duration)
                     .setEase(easeType)
                     .setOnUpdate(val => _targetMaterial.SetFloat("_BorderOffsetX", val));

            float currentY = _targetMaterial.GetFloat("_BorderOffsetY");
            LeanTween.value(gameObject, currentY, targetY, duration)
                     .setEase(easeType)
                     .setOnUpdate(val => _targetMaterial.SetFloat("_BorderOffsetY", val));
        }
    }
}

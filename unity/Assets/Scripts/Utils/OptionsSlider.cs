using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class OptionsSlider : MonoBehaviour
    {
        public RawImage sliderImage;
        public Slider slider;

        void Start()
        {
            sliderImage.material = new Material(sliderImage.material);
            slider.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(slider.value);
        }

        void OnValueChanged(float v)
        {
            float mapped = Mathf.Lerp(0.07f, 0.93f, v/slider.maxValue);
            sliderImage.material.SetFloat("_InnerFillAmount", mapped);
        }
    }
}
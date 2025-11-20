using Core.Prefab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Analysis
{
    public class ThreatOfDiseasePrefab : MonoBehaviour
    {
        public Image threatIconImage;
        public TextMeshProUGUI threatLevelText;

        public void Setup(Sprite _threatIconImage, int threatLevel)
        {
            threatIconImage.sprite = _threatIconImage;
            threatLevelText.text = threatLevel.ToString();
            if (threatLevel > 0)
            {
                threatLevelText.color = Color.green;
            }
            else
            {
                threatLevelText.color = Color.red;
            }
        }
    }
}
using Core.Threats;
using UnityEngine;

namespace Core.Item
{
    [CreateAssetMenu(fileName = "BacteriaItem", menuName = "Item/BacteriaItem")]
    public class BacteriaItem : Item
    {
        public ThreatParameters threatParameters;
    }
}
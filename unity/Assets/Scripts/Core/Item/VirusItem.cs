using System;
using System.Linq;
using Core.Threats;
using Core.Item.Holder;
using UnityEngine;

namespace Core.Item
{
    [CreateAssetMenu(fileName = "VirusItem", menuName = "Item/VirusItem")]
    public class VirusItem : Item
    {
        [SerializeField]
        public ThreatParameter[] threatParameters;
        
        public override HoldItem GetHoldItem()
        {
            return new HoldVirusItem(this);
        }
    }
    
    [Serializable]
    public class ThreatParameter
    {
        public ThreatType threatType;
        public int threatImpact;
    }
}
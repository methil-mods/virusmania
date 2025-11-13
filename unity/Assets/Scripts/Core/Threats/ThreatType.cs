using UnityEngine;

namespace Core.Threats
{
    [CreateAssetMenu(fileName = "ThreatType", menuName = "Threats/ThreatType")]
    public class ThreatType: ScriptableObject
    {
        public string treatTypeName;
    }
}
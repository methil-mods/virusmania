using System.Collections.Generic;
using UnityEngine;

namespace Core.Threats
{
    [CreateAssetMenu(fileName = "ThreatTypeDatabase", menuName = "Threats/ThreatTypeDatabase")]
    public class ThreatTypeDatabase: ScriptableObject
    {
        public string databaseName;
        public List<ThreatType> threatTypes;
        public List<ThreatType> transmissionFactors;
    }
}
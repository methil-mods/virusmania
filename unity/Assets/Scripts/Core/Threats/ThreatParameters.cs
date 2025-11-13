using System;
using UnityEngine;

namespace Core.Threats
{
    [CreateAssetMenu(fileName = "ThreatParameters", menuName = "Threats/ThreatParameters")]
    public class ThreatParameters: ScriptableObject
    {
        public ThreatType threatType;
        public float value;
    }
}
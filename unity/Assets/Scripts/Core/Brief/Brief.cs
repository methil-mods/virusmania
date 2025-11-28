using UnityEngine;

namespace Core.Brief
{
    [CreateAssetMenu(menuName = "Brief/Brief", fileName = "Brief")]
    public class Brief : ScriptableObject
    {
        public string briefTitle;
        [TextArea]
        public string briefDescription;
        public Item.Item wantedItem;
        public bool onBoarding = false;

        [Min(0)]
        public int moneyGiven;

        [Min(0)]
        public int timeForBrief;
        
        public BriefDifficulty difficulty;
    }

    public enum BriefDifficulty
    {
        Easy,
        Medium,
        Hard,
        OnBoarding
    }
}
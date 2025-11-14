using UnityEngine;
using TMPro;

namespace Core.Timer
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;

        private void Update()
        {
            float duration = TimerController.Instance.TimerDuration;
            float current = TimerController.Instance.CurrentTime;

            if (duration <= 0f) return;

            float timeRemaining = Mathf.Max(duration - current, 0f);
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
using System.Threading.Tasks;
using Framework.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Timer
{
    public class TimerController : BaseController<TimerController>
    {
        public float CurrentTime { get; private set; }
        public float TimerDuration { get; private set; }

        public UnityAction OnTimerEnd;
        public UnityAction<float> OnTimerTick;

        private bool _isRunning;
        private bool _stopRequested;

        public async void LaunchTimer(float duration)
        {
            if (_isRunning) return;

            TimerDuration = duration;
            CurrentTime = 0f;
            _isRunning = true;
            _stopRequested = false;

            while (CurrentTime < duration)
            {
                if (_stopRequested) break;

                await Task.Yield();
                CurrentTime += Time.deltaTime;
                OnTimerTick?.Invoke(CurrentTime);
            }

            if (!_stopRequested)
                OnTimerEnd?.Invoke();

            _isRunning = false;
            CurrentTime = 0f;
            TimerDuration = 0f;
        }

        public void StopTimer()
        {
            if (!_isRunning) return;
            _stopRequested = true;
        }
    }
}
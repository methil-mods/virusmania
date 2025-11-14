using System;
using System.Threading.Tasks;
using Framework.Controller;
using UnityEngine;

namespace Core.Timer
{
    public class TimerController : BaseController<TimerController>
    {
        public float CurrentTime { get; private set; }
        public float TimerDuration { get; private set; }
        private bool isRunning;
        private bool stopRequested;

        public async void LaunchTimer(float duration, Action callback)
        {
            if (isRunning) return;

            TimerDuration = duration;
            CurrentTime = 0f;
            isRunning = true;
            stopRequested = false;

            while (CurrentTime < duration)
            {
                if (stopRequested) break;

                await Task.Yield();
                CurrentTime += Time.deltaTime;
            }

            if (!stopRequested)
                callback?.Invoke();

            isRunning = false;
            CurrentTime = 0f;
            TimerDuration = 0f;
        }

        public void StopTimer()
        {
            if (!isRunning) return;

            stopRequested = true;
        }
    }
}
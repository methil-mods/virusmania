using System;
using Framework.Controller;
using UnityEngine;
using System.Collections.Generic;
using Core.Timer;

namespace Core.SFX
{
    public class SFXController : BaseController<SFXController>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private int initialPoolSize = 5;

        private readonly List<AudioSource> interactionPool = new List<AudioSource>();
        private readonly List<AudioSource> uiPool = new List<AudioSource>();

        private bool music1Played = false;
        private bool music2Played = false;
        private bool music3Played = false;

        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateInteractionSource();
                CreateUISource();
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SFXDatabase.Instance.SetupSound();

            PlayMusic(SFXDatabase.Instance.musicClip1);
            music1Played = true;

            if (TimerController.Instance != null)
                TimerController.Instance.OnTimerTick += OnTimerTick;
        }

        private void Update()
        {
            if (TimerController.Instance == null)
            {
                ForceMusic1();
                return;
            }

            if (TimerController.Instance.TimerDuration == 0f)
            {
                ForceMusic1();
                return;
            }
        }

        private void OnDestroy()
        {
            if (TimerController.Instance != null)
                TimerController.Instance.OnTimerTick -= OnTimerTick;
        }

        private void OnTimerTick(float elapsed)
        {
            if (TimerController.Instance == null)
            {
                ForceMusic1();
                return;
            }

            float remaining = TimerController.Instance.TimerDuration - elapsed;

            if (remaining <= 120f && !music1Played)
            {
                PlayMusic(SFXDatabase.Instance.musicClip1);
                music1Played = true;
            }

            if (remaining <= 60f && !music2Played)
            {
                PlayMusic(SFXDatabase.Instance.musicClip2);
                music2Played = true;
            }

            if (remaining <= 30f && !music3Played)
            {
                PlayMusic(SFXDatabase.Instance.musicClip3);
                music3Played = true;
            }
        }

        public void ResetMusicState()
        {
            music1Played = false;
            music2Played = false;
            music3Played = false;
        }

        private void ForceMusic1()
        {
            if (!music1Played)
            {
                PlayMusic(SFXDatabase.Instance.musicClip1);
                music1Played = true;
            }
        }

        private AudioSource CreateInteractionSource()
        {
            var go = new GameObject("InteractionSFX");
            go.transform.SetParent(transform);
            var s = go.AddComponent<AudioSource>();
            s.outputAudioMixerGroup = SFXDatabase.Instance.interactionAudioGroup;
            interactionPool.Add(s);
            return s;
        }

        private AudioSource CreateUISource()
        {
            var go = new GameObject("UISFX");
            go.transform.SetParent(transform);
            var s = go.AddComponent<AudioSource>();
            s.outputAudioMixerGroup = SFXDatabase.Instance.userInterfaceAudioGroup;
            uiPool.Add(s);
            return s;
        }

        private AudioSource GetFree(List<AudioSource> pool, Func<AudioSource> create)
        {
            foreach (var s in pool)
                if (!s.isPlaying)
                    return s;

            return create();
        }

        public void PlayInteraction(AudioClip clip)
        {
            if (clip == null) return;
            var s = GetFree(interactionPool, CreateInteractionSource);
            s.PlayOneShot(clip);
        }

        public void PlayUI(AudioClip clip)
        {
            if (clip == null) return;
            var s = GetFree(uiPool, CreateUISource);
            s.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null) return;
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}

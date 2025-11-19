using System;
using Framework.Controller;
using UnityEngine;
using System.Collections.Generic;

namespace Core.SFX
{
    public class SFXController : BaseController<SFXController>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private int initialPoolSize = 5;

        private readonly List<AudioSource> interactionPool = new List<AudioSource>();
        private readonly List<AudioSource> uiPool = new List<AudioSource>();

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
            SFXDatabase.instance.SetupSound();
            PlayMusic(SFXDatabase.instance.musicClip1);
        }

        private AudioSource CreateInteractionSource()
        {
            var go = new GameObject("InteractionSFX");
            go.transform.SetParent(transform);
            var s = go.AddComponent<AudioSource>();
            s.outputAudioMixerGroup = SFXDatabase.instance.interactionAudioGroup;
            interactionPool.Add(s);
            return s;
        }

        private AudioSource CreateUISource()
        {
            var go = new GameObject("UISFX");
            go.transform.SetParent(transform);
            var s = go.AddComponent<AudioSource>();
            s.outputAudioMixerGroup = SFXDatabase.instance.userInterfaceAudioGroup;
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

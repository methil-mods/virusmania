using Framework.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SFXDatabase", menuName = "SFX/SFXDatabase")]
public class SFXDatabase : SingletonScriptableObject<SFXDatabase>
{
    [Header("Splashscreen")]
    public AudioClip splashScreen;
    
    [Header("UI Audio Clip Database")]
    public AudioClip popUiClip;
    public AudioClip clickUiClip;

    public AudioClip noMoneyClip;
    
    [Header("World Audio Clip Database")]
    public AudioClip walkClip;
    
    public AudioClip musicClip1;
    public AudioClip musicClip2;
    public AudioClip musicClip3;

    public AudioClip openCookClip;
    public AudioClip boilingCookClip;
    public AudioClip endCookClip;

    public AudioClip mergeAudioClip;

    public AudioClip triggerTrashClip;

    public AudioClip leverClip;

    public AudioClip flapOpenClip;

    public AudioClip redAlarmClip;
    public AudioClip greenAlarmClip;
    
    [Header("Volume and Audio Group")]
    [Range(0, 100)] public float musicVolume;
    [Range(0, 100)] public float interactionVolume;
    [Range(0, 100)] public float uiVolume;

    public AudioMixerGroup musicAudioGroup;
    public AudioMixerGroup interactionAudioGroup;
    public AudioMixerGroup userInterfaceAudioGroup;

    float Map(float value) => Mathf.Lerp(-80f, 20f, value / 100f);

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            musicAudioGroup.audioMixer.SetFloat("MusicVolume", Map(value));
        }
    }

    public float InteractionVolume
    {
        get => interactionVolume;
        set
        {
            interactionVolume = value;
            interactionAudioGroup.audioMixer.SetFloat("InteractionVolume", Map(value));
        }
    }

    public float UserInterfaceVolume
    {
        get => uiVolume;
        set
        {
            uiVolume = value;
            userInterfaceAudioGroup.audioMixer.SetFloat("UserInterfaceVolume", Map(value));
        }
    }

    public void SetupSound()
    {
        UserInterfaceVolume = uiVolume;
        InteractionVolume = interactionVolume;
        MusicVolume = musicVolume;
    }
}
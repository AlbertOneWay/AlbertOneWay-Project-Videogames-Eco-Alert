using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public enum MusicType { Calm1, Calm2, Enemy }
public enum UISFXType { Click, Notification, Cash }

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Referencias")]
    public AudioMixer audioMixer;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;
    
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup uiGroup;

    [Header("🎵 Clips de Música")]
    public AudioClip musicCalm1;
    public AudioClip musicCalm2;
    public AudioClip musicEnemy;

    [Header("🧩 Sonidos UI")]
    public AudioClip uiClick;
    public AudioClip uiNotification;
    public AudioClip uiCash;

    private Dictionary<MusicType, AudioClip> musicClips;
    private Dictionary<UISFXType, AudioClip> uiClips;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        musicClips = new Dictionary<MusicType, AudioClip>
        {
            { MusicType.Calm1, musicCalm1 },
            { MusicType.Calm2, musicCalm2 },
            { MusicType.Enemy, musicEnemy }
        };

        uiClips = new Dictionary<UISFXType, AudioClip>
        {
            { UISFXType.Click, uiClick },
            { UISFXType.Notification, uiNotification },
            { UISFXType.Cash, uiCash }
        };

        if (musicGroup != null) musicSource.outputAudioMixerGroup = musicGroup;
        if (sfxGroup != null) sfxSource.outputAudioMixerGroup = sfxGroup;
        if (uiGroup != null) uiSource.outputAudioMixerGroup = uiGroup;
        
    }
    
    private void Start()
    {
        PlayRandomCalmMusic(); // o PlayMusic(MusicType.Calm1);
        
        // 🎚️ Volúmenes por defecto
        SetVolume("MusicVolume", 0.01f); // 40% de volumen
        SetVolume("SFXVolume", 1f);     // 100%
        SetVolume("UIVolume", 1f);      // 100%
    }

    // Música por tipo
    public void PlayMusic(MusicType type, bool loop = true)
    {
        if (!musicClips.ContainsKey(type) || musicClips[type] == null)
        {
            Debug.LogWarning($"🎵 No hay música para: {type}");
            return;
        }

        musicSource.clip = musicClips[type];
        musicSource.loop = loop;
        musicSource.Play();
    }
    
    public AudioSource PlayLoopingSFX(AudioClip clip, string name = "LoopingSFX")
    {
        if (clip == null) return null;

        GameObject go = new GameObject(name);
        go.transform.parent = this.transform;

        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

        source.Play();
        return source;
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayRandomCalmMusic()
    {
        MusicType randomType = (Random.value < 0.5f) ? MusicType.Calm1 : MusicType.Calm2;
        PlayMusic(randomType);
    }

    public void SetVolume(string exposedParam, float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
        
        float currentValue;
        audioMixer.GetFloat(exposedParam, out currentValue);
        Debug.Log($"[AudioManager] {exposedParam} set to {currentValue} dB");
    }

    // SFX 2D
    public void PlaySFX2D(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource source = CreateTemporaryAudioSource(clip, "SFX2D");
        source.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;
        source.spatialBlend = 0f;
    }

    // SFX 3D
    public void PlaySFX3D(AudioClip clip, Vector3 position, float spatialBlend = 1f)
    {
        if (clip == null) return;

        GameObject go = new GameObject($"OneShot_SFX3D_{clip.name}");
        go.transform.position = position;
        go.transform.parent = this.transform;

        AudioSource tempSource = go.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.spatialBlend = Mathf.Clamp01(spatialBlend);
        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;
        tempSource.minDistance = 1f;
        tempSource.maxDistance = 20f;
        tempSource.rolloffMode = AudioRolloffMode.Linear;

        tempSource.Play();
        Destroy(go, clip.length + 0.1f);
    }

    // UI por tipo
    public void PlayUI(UISFXType type)
    {
        if (!uiClips.ContainsKey(type) || uiClips[type] == null)
        {
            Debug.LogWarning($"🔊 Sonido UI no encontrado: {type}");
            return;
        }

        AudioSource source = CreateTemporaryAudioSource(uiClips[type], "UI");
        source.outputAudioMixerGroup = uiSource.outputAudioMixerGroup;
    }

    private AudioSource CreateTemporaryAudioSource(AudioClip clip, string prefix)
    {
        GameObject go = new GameObject($"OneShot_{prefix}_{clip.name}");
        go.transform.parent = this.transform;
        

        AudioSource tempSource = go.AddComponent<AudioSource>();
        tempSource.outputAudioMixerGroup = prefix switch
        {
            "SFX2D" => sfxGroup,
            "SFX3D" => sfxGroup,
            "UI" => uiGroup,
            _ => null
        };
        tempSource.clip = clip;
        tempSource.playOnAwake = false;
        tempSource.spatialBlend = 0f;
        tempSource.loop = false;

        tempSource.Play();
        Destroy(go, clip.length + 0.1f);

        return tempSource;
    }
    
    public void StopAndDestroy(AudioSource source)
    {
        if (source == null) return;

        source.Stop();
        Destroy(source.gameObject);
    }
}

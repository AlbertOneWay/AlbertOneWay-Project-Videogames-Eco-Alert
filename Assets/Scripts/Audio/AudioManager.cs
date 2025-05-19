using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Referencias")]
    public AudioMixer audioMixer;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicClip == null)
        {
            Debug.LogWarning("🎵 No hay música para reproducir.");
            return;
        }

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetVolume(string exposedParam, float sliderValue)
    {
        // Convierte [0,1] en decibelios [-80, 0]
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }
    
    public void PlaySFX2D(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource source = CreateTemporaryAudioSource(clip, "SFX2D");
        source.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;
        source.spatialBlend = 0f; // 2D puro
    }

    public void PlaySFX3D(AudioClip clip, Vector3 position, float spatialBlend = 1f)
    {
        if (clip == null) return;

        GameObject go = new GameObject($"OneShot_SFX3D_{clip.name}");
        go.transform.position = position;
        go.transform.parent = this.transform;

        AudioSource tempSource = go.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.spatialBlend = Mathf.Clamp01(spatialBlend); // 0 = 2D, 1 = 3D
        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;

        tempSource.minDistance = 1f;
        tempSource.maxDistance = 20f;
        tempSource.rolloffMode = AudioRolloffMode.Linear;

        tempSource.Play();
        Destroy(go, clip.length + 0.1f);
    }

    public void PlayUI(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource source = CreateTemporaryAudioSource(clip, "UI");
        source.outputAudioMixerGroup = uiSource.outputAudioMixerGroup;
    }

    private AudioSource CreateTemporaryAudioSource(AudioClip clip, string prefix)
    {
        GameObject go = new GameObject($"OneShot_{prefix}_{clip.name}");
        go.transform.parent = this.transform;

        AudioSource tempSource = go.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.playOnAwake = false;
        tempSource.spatialBlend = 0f;
        tempSource.loop = false;

        tempSource.Play();
        Destroy(go, clip.length + 0.1f); // Se elimina cuando termina

        return tempSource;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [HideInInspector] public bool musicEnabled = true;
    [HideInInspector] public bool sfxEnabled = true;

    public bool IsSFXMuted => sfxSource.mute;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        switch (scene.name)
        {
            case "Garage Scene":
                musicSource.Play(); 
                musicSource.mute = !musicEnabled; 
                break;
            case "Day Scene":
                musicSource.Stop();
                break;
            case "Night Scene":
                musicSource.Stop();
                break;
        }
    }

    public void SetMusic(bool enabled)
    {
        musicEnabled = enabled;
        musicSource.mute = !enabled;

        PlayerPrefs.SetInt("Music", enabled ? 1 : 0);
    }

    public void SetSFX(bool enabled)
    {
        sfxEnabled = enabled;
        sfxSource.mute = !enabled;

        PlayerPrefs.SetInt("SFX", enabled ? 1 : 0);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void ToggleMusic(bool value)
    {
        SetMusic(value);
    }

    public void ToggleSFX(bool value)
    {
        SetSFX(value);
    }

    void LoadSettings()
    {
        musicEnabled = PlayerPrefs.GetInt("Music", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFX", 1) == 1;

        musicSource.mute = !musicEnabled;
        sfxSource.mute = !sfxEnabled;
    }
}

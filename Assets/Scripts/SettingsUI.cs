using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;

    void OnEnable()
    {
        musicToggle.onValueChanged.RemoveAllListeners();
        sfxToggle.onValueChanged.RemoveAllListeners();

        musicToggle.onValueChanged.AddListener(AudioManager.Instance.ToggleMusic);
        sfxToggle.onValueChanged.AddListener(AudioManager.Instance.ToggleSFX);

        musicToggle.SetIsOnWithoutNotify(AudioManager.Instance.musicEnabled);
        sfxToggle.SetIsOnWithoutNotify(AudioManager.Instance.sfxEnabled);
    }
}
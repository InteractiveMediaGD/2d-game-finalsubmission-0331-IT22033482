using UnityEngine;
using UnityEngine.UI;

public class MenuSettingsController : MonoBehaviour
{
    private const string KeyMusicVol = "WingsOfAsh_MusicVol";
    private const string KeySfxVol = "WingsOfAsh_SfxVol";

    [Header("Music")]
    [SerializeField] private Slider musicSlider;

    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;

    private bool _listenersAdded;

    private void OnEnable()
    {
        if (!_listenersAdded)
        {
            _listenersAdded = true;
            if (musicSlider != null)
            {
                musicSlider.onValueChanged.AddListener(OnMusicVolume);
            }

            if (sfxSlider != null)
            {
                sfxSlider.onValueChanged.AddListener(OnSfxVolume);
            }
        }

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(KeyMusicVol, 0.75f));
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(KeySfxVol, 0.9f));
        }
    }

    private void OnMusicVolume(float v)
    {
        PlayerPrefs.SetFloat(KeyMusicVol, v);
        PlayerPrefs.Save();
    }

    private void OnSfxVolume(float v)
    {
        PlayerPrefs.SetFloat(KeySfxVol, v);
        PlayerPrefs.Save();
    }
}

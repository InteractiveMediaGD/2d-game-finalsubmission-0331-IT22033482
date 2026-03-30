using UnityEngine;

// Looping BGM; volume follows main menu music slider (PlayerPrefs WingsOfAsh_MusicVol).
[DisallowMultipleComponent]
public class WingsOfAshMusic : MonoBehaviour
{
    public static WingsOfAshMusic Instance { get; private set; }

    private const string KeyMusicVol = "WingsOfAsh_MusicVol";

    [Header("Music (e.g. Assets/WingsOfAsh/Audio/Music/background)")]
    [SerializeField] private AudioClip musicClip;

    [SerializeField] [Range(0f, 1f)] private float volumeMultiplier = 1f;

    private AudioSource _musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameObject musicGo = new GameObject("WingsOfAshMusic_Audio");
        musicGo.transform.SetParent(transform, false);
        _musicSource = musicGo.AddComponent<AudioSource>();
        _musicSource.playOnAwake = false;
        _musicSource.loop = true;
        _musicSource.spatialBlend = 0f;
        _musicSource.clip = musicClip;

        ApplyVolume();

        if (musicClip != null)
        {
            _musicSource.Play();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (_musicSource == null)
        {
            return;
        }

        ApplyVolume();
    }

    private void ApplyVolume()
    {
        float slider = PlayerPrefs.GetFloat(KeyMusicVol, 0.75f);
        _musicSource.volume = Mathf.Clamp01(volumeMultiplier * slider);
    }
}

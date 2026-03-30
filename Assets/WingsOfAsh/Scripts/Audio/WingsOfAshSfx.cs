using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Plays one-shot SFX (death, fire, UI clicks, pickup). Add one instance to the first scene (e.g. 00_Loading)
/// and assign clips from Assets/WingsOfAsh/Audio/SFX. Wires all <see cref="Button"/> clicks in each loaded scene.
/// </summary>
[DisallowMultipleComponent]
public class WingsOfAshSfx : MonoBehaviour
{
    public static WingsOfAshSfx Instance { get; private set; }

    private const string KeySfxVol = "WingsOfAsh_SfxVol";

    [Header("Clips (drag from Assets/WingsOfAsh/Audio/SFX)")]
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip fireShootClip;
    [SerializeField] private AudioClip pickupClip;

    [SerializeField] [Range(0f, 1f)] private float volume = 1f;

    private AudioSource _audioSource;
    private readonly HashSet<int> _wiredButtonInstanceIds = new HashSet<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void Start()
    {
        WireButtonsInScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        WireButtonsInScene(scene);
    }

    private void WireButtonsInScene(Scene scene)
    {
        if (!scene.IsValid() || !scene.isLoaded)
        {
            return;
        }

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Button[] buttons = root.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                Button b = buttons[i];
                if (b == null)
                {
                    continue;
                }

                int id = b.GetInstanceID();
                if (_wiredButtonInstanceIds.Contains(id))
                {
                    continue;
                }

                _wiredButtonInstanceIds.Add(id);
                b.onClick.AddListener(PlayButtonClick);
            }
        }
    }

    public void PlayDeath()
    {
        PlayClip(deathClip);
    }

    public void PlayButtonClick()
    {
        PlayClip(buttonClickClip);
    }

    public void PlayFireShoot()
    {
        PlayClip(fireShootClip);
    }

    public void PlayPickup()
    {
        PlayClip(pickupClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || _audioSource == null)
        {
            return;
        }

        float slider = PlayerPrefs.GetFloat(KeySfxVol, 0.9f);
        _audioSource.PlayOneShot(clip, Mathf.Clamp01(volume * slider));
    }
}

using UnityEngine;

/// <summary>
/// Sound Manager to play audio clips upon request.
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Header("Is On")]
    [Space(5)]
    [SerializeField] private bool isOn;

    [Space(10)]

    [Header("Game Sounds")]
    [Space(5)]
    public AudioClip throwSound;
    public AudioClip dropSound;
    public AudioClip grabSound;
    public AudioClip scoreSound;
    public AudioClip clickSound;
    public AudioClip gameOverSound;

    [Space(10)]

    [Header("Sounds Audio Source")]
    [Space(5)]
    [SerializeField] private AudioSource audioSource;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySound(AudioClip audioClip)
    {
        if (!isOn) return;
        
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clipToPlay = clips[randomIndex];

        _audioSource.PlayOneShot(clipToPlay);
    }

    public void PlaySoundWithRandomPitch(AudioClip clip, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (clip == null) return;

        _audioSource.pitch = Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(clip);
        _audioSource.pitch = 1f;
    }
}

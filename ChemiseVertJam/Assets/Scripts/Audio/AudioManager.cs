using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRandomSound(AudioSource audioSource, AudioClip[] clips)
    {
        if (clips.Length == 0 || audioSource == null) return;

        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clipToPlay = clips[randomIndex];

        audioSource.PlayOneShot(clipToPlay);
    }

    public void PlaySoundWithRandomPitch(AudioSource audioSource, AudioClip clip, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (clip == null || audioSource == null) return;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
        audioSource.pitch = 1f;
    }
}

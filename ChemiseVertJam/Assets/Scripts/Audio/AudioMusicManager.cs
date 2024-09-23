using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMusicManager : MonoBehaviour
{
    public static AudioMusicManager Instance { get; private set; }

    public AudioClip[] ambiance1;
    public AudioClip[] ambiance2;
    public AudioSource audioSource;
    private string currentMusicPlaying = "";

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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void PlayMusicForLevel(int level)
    {
        AudioClip[] musicToPlay = null;

        if (level >= 1 && level <= 3)
        {
            musicToPlay = ambiance1;
        }
        else if (level >= 4 && level <= 6)
        {
            musicToPlay = ambiance2;
        }

        if (musicToPlay != null && musicToPlay.Length > 0)
        {
            AudioClip selectedClip = musicToPlay[Random.Range(0, musicToPlay.Length)];

            if (currentMusicPlaying != selectedClip.name)
            {
                audioSource.clip = selectedClip;
                audioSource.Play();
                currentMusicPlaying = selectedClip.name;
            }
        }
    }
}

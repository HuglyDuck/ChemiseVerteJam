using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstance : MonoBehaviour
{
    public AudioClip[] soundClips;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (soundClips.Length > 0)
        {
            AudioManager.Instance.PlayRandomSound(audioSource, soundClips);
        }
    }
}

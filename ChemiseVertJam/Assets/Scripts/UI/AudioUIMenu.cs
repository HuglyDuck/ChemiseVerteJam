using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUIMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource; 
    [SerializeField] private AudioClip _selectSoundClip; 
    [SerializeField] private AudioClip _clickSoundClip; 

    

    public void PlaySelectSound()
    {
        if (_audioSource != null && _selectSoundClip != null)
        {
            _audioSource.PlayOneShot(_selectSoundClip);
        }
        
    }

    public void PlayClickSound()
    {
        if (_audioSource != null && _clickSoundClip != null)
        {
            _audioSource.PlayOneShot(_clickSoundClip);
        }
        
    }
}

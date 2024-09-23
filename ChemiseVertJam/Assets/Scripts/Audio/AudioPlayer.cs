using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] _audioClipsSteps;

    [SerializeField] AudioSource _audioSourceStep;
    [SerializeField] float _volumeStep;
    [SerializeField] float _minSpeedStep;
    [SerializeField] float _SpeedStep;

    private bool _isPlaying = true;
    private float _timerStep;
    

    private void Update()
    {
        if (_isPlaying && PlayerMovement.speedPourcentage > 0)
        {
            _isPlaying = false;
            _audioSourceStep.volume = PlayerMovement.speedPourcentage * _volumeStep;
            _timerStep = 0;
            AudioManager.Instance.PlayRandomSound(_audioSourceStep, _audioClipsSteps);
        }
        else if (_timerStep >= 1) _isPlaying = true;
        else _timerStep += Time.deltaTime * (_minSpeedStep + (PlayerMovement.speedPourcentage * _SpeedStep));
    }
}

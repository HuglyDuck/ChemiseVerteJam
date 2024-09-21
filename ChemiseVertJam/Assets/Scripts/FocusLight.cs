using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusLight : MonoBehaviour
{
    [SerializeField] float _timerFocusLight;
    [SerializeField] PlayerMovement _scriptPlayerMovement;
    [SerializeField] float _focusDuration;

    private ESneeze _eSneeze = ESneeze.Timer;

    private float _timerValue;
    private bool _enabled = false;
    private bool _startTimer = true;

    public event Action _eventSneeze;

    private void OnEnable()
    {
        _scriptPlayerMovement._eventStartMove += _scriptPlayerMovement__firstRunEvent;
    }

    private void _scriptPlayerMovement__firstRunEvent()
    {
        _enabled = true;
    }

    private void Update()
    {
        if (_enabled)
        {
            switch (_eSneeze)
            {
                case ESneeze.Timer:
                    Timer(_timerFocusLight, ESneeze.Duration);
                    break;
                case ESneeze.Duration:
                    Timer(_focusDuration, ESneeze.Timer);
                    break;
            }
        }
        
    }

    private void Timer(float duration,ESneeze eSneeze)
    {
        
        if (_startTimer)
        {
            _startTimer = false;
            _timerValue = Time.time + duration;
        }
        if (!_startTimer && Time.time >= _timerValue)
        {
            _startTimer = true;
            _eventSneeze?.Invoke();
            _eSneeze = eSneeze;
        }

        
    }

    private void OnDisable()
    {
        _scriptPlayerMovement._eventStartMove -= _scriptPlayerMovement__firstRunEvent;
    }

    enum ESneeze
    {
        Timer,
        Duration
    }
}

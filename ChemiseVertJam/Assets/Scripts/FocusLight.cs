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

    private float _currentTime;

    private ESneeze _eSneeze = ESneeze.Timer;

    private float _timerValue;
    private bool _enabled = false;
    private bool _startTimer = true;

    public event Action _eventSneeze;

    private void OnEnable()
    {
        _scriptPlayerMovement._eventStartMove += _scriptPlayerMovement__firstRunEvent;
    }

    private void Start()
    {
        Shader.SetGlobalFloat("_ValueGauge", 1);
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
                    if (_startTimer)
                    {
                        _startTimer = false;
                        _timerValue = Time.time + _timerFocusLight;
                        _currentTime = 1;
                    }
                    else _currentTime = (_timerValue - Time.time) / _timerFocusLight;

                    Shader.SetGlobalFloat("_ValueGauge", _currentTime);

                    if (!_startTimer && Time.time >= _timerValue)
                    {
                        _startTimer = true;
                        _eventSneeze?.Invoke();
                        _eSneeze = ESneeze.Duration;
                    }
                    break;
                case ESneeze.Duration:
                    if (_startTimer)
                    {
                        _startTimer = false;
                        _timerValue = Time.time + _focusDuration;
                    }
                    else

                    if (!_startTimer && Time.time >= _timerValue)
                    {
                        _startTimer = true;
                        _eventSneeze?.Invoke();
                        _eSneeze = ESneeze.Timer;
                    }
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
        else

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

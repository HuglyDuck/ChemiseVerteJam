using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 4f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _moveTowardSpeed = 1f;
    [SerializeField] private SplineMovement _scriptSplineMovement;
    private float _targetSpeed;
    private float _currentSpeed;
    private int _speedIndex = 0;
    private GameInputs _inputActions;

    public event Action _eventStartMove;
    private bool _startMove = true;

    private void Awake()
    {
        _inputActions = new GameInputs();
    }

    private void OnEnable()
    {
        _inputActions.InGame.StopPlayer.Enable();
        _inputActions.InGame.RunPlayer.Enable();
        _inputActions.InGame.RunPlayer.performed += RunPlayer_performed;
        _inputActions.InGame.StopPlayer.performed += StopPlayer_performed;
    }

    private void RunPlayer_performed(InputAction.CallbackContext context)
    {
        if (_speedIndex == 2)
        {
            _speedIndex = 1;
            _targetSpeed = _speed;
        }
        else
        {
            _speedIndex = 2;
            _targetSpeed = _runSpeed;
        }

        if (_startMove)
        {
            _startMove = false;
            _eventStartMove?.Invoke();
        }
    }

    private void StopPlayer_performed(InputAction.CallbackContext context)
    {
        if (_speedIndex == 0)
        {
            _speedIndex = 1;
            _targetSpeed = _speed;
        }
        else
        {
            _speedIndex = 0;
            _targetSpeed = 0;
        }

        if(_startMove)
        {
            _startMove = false;
            _eventStartMove?.Invoke();
        }
    }

    private void Update()
    {
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, _moveTowardSpeed * Time.deltaTime);
        _scriptSplineMovement._currentSpeed = _currentSpeed;
    }

    private void OnDisable()
    {
        _inputActions.InGame.StopPlayer.Enable();
        _inputActions.InGame.RunPlayer.Enable();
        _inputActions.InGame.RunPlayer.performed -= RunPlayer_performed;
        _inputActions.InGame.StopPlayer.performed -= StopPlayer_performed;
    }
}

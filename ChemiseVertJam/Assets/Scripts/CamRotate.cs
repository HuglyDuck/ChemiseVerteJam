using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamRotate : MonoBehaviour
{
    private GameInputs _inputActions;
    private float _hValue = .5f;
    private float _vValue = .5f;

    [SerializeField] private float _maxHValue;
    [SerializeField] private float _maxVValue;

    private float _currentValueH;
    private float _currentValueV;

    private Quaternion _baseRotate;

    private void Awake()
    {
        _inputActions = new GameInputs();
    }

    private void Start()
    {
        _baseRotate = Camera.main.transform.rotation;
    }

    private void OnEnable()
    {
        _inputActions.InGame.RotateCamH.Enable();
        _inputActions.InGame.RotateCamV.Enable();
        _inputActions.InGame.RotateCamH.performed += RotateCamH_performed;
        _inputActions.InGame.RotateCamH.canceled += RotateCamH_canceled;
        _inputActions.InGame.RotateCamV.performed += RotateCamV_performed;
        _inputActions.InGame.RotateCamV.canceled += RotateCamV_canceled;
    }

    private void OnDisable()
    {
        _inputActions.InGame.RotateCamH.Disable();
        _inputActions.InGame.RotateCamV.Disable();
        _inputActions.InGame.RotateCamH.performed -= RotateCamH_performed;
        _inputActions.InGame.RotateCamH.canceled -= RotateCamH_canceled;
        _inputActions.InGame.RotateCamV.performed -= RotateCamV_performed;
        _inputActions.InGame.RotateCamV.canceled -= RotateCamV_canceled;
    }

    private void RotateCamV_performed(InputAction.CallbackContext context)
    {
        _vValue = context.ReadValue<float>();
        _vValue += 1;
        _vValue *= .5f;
    }

    private void RotateCamV_canceled(InputAction.CallbackContext context)
    {
        _vValue = .5f;
    }

    private void RotateCamH_performed(InputAction.CallbackContext context)
    {
        _hValue = context.ReadValue<float>();
        _hValue += 1;
        _hValue *= .5f;
    }

    private void RotateCamH_canceled(InputAction.CallbackContext context)
    {
        _hValue = .5f; 
    }

    private void Update()
    {
        _currentValueH = Mathf.Lerp(-_maxHValue, _maxHValue, _hValue);
        _currentValueV = Mathf.Lerp(_maxVValue,-_maxVValue, _vValue);

        Camera.main.transform.rotation = _baseRotate * Quaternion.Euler(_currentValueV, _currentValueH, 0);
    }
}

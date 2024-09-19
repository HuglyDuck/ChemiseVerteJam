using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    GameInputs _inputActions;
    private float _selectValue;
    [SerializeField] private List<GameObject> _interactObjectList = new List<GameObject>();
    private int _objectValue;
    

    private void Awake()
    {
        _inputActions = new GameInputs();
    }

    private void OnEnable()
    {
        _inputActions.InGame.Enable();
        _selectValue = _inputActions.InGame.SelectObject.ReadValue<float>();
        _inputActions.InGame.SelectObject.performed += SelectObject_performed;
    }

    private void SelectObject_performed(InputAction.CallbackContext context)
    {
        if(_selectValue > 0)
        {
            SelectObjectAdd();

        }
        else
        {
            SelectObjectSub();
        }
    }

    private void SelectObjectAdd()
    {
        if (_objectValue <= _interactObjectList.Count - 1)
        {
            _objectValue = _interactObjectList.Count - 1;
        }
        else
        {

            _objectValue++;
        }
    }

    private void SelectObjectSub()
    {
        if (_objectValue >= 0)
        {
            _objectValue = 0;
        }
        else _objectValue--;
    }



    private void OnDisable()
    {
        _inputActions.InGame.Disable();
        _inputActions.InGame.SelectObject.performed -= SelectObject_performed;
    }
}

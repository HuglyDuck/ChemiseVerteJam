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

    private void Start()
    {
        foreach (GameObject obj in _interactObjectList)
        {
            OnDesactive(obj);
        }

        OnActive(_interactObjectList[_objectValue]);
    }

    private void OnEnable()
    {
        _inputActions.InGame.Enable();
        
        _inputActions.InGame.SelectObject.performed += SelectObject_performed;
    }

    private void SelectObject_performed(InputAction.CallbackContext context)
    {
        _selectValue = _inputActions.InGame.SelectObject.ReadValue<float>();

        if (_selectValue > 0)
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
        OnDesactive(_interactObjectList[_objectValue]);
        if (_objectValue >= _interactObjectList.Count - 1)
        {
            _objectValue = 0;
        }
        else _objectValue++;
        OnActive(_interactObjectList[_objectValue]);
    }

    private void SelectObjectSub()
    {
        OnDesactive(_interactObjectList[_objectValue]);
        if (_objectValue <= 0)
        {
            _objectValue = _interactObjectList.Count - 1;
        }
        else _objectValue--;
        OnActive(_interactObjectList[_objectValue]);
        
    }

    private void OnActive(GameObject _interactObject)
    {
       if( _interactObject.TryGetComponent(out ActivateSelected _scriptActivateSelected))
        {
            _scriptActivateSelected.Activate();
        }
    }

    private void OnDesactive(GameObject _interactObject)
    {
        if (_interactObject.TryGetComponent(out ActivateSelected _scriptActivateSelected))
        {
            _scriptActivateSelected.Desactivate();
        }
    }
    private void OnSelectedObject(InputAction.CallbackContext context)
    {
        
    }

    private void OnDisable()
    {
        _inputActions.InGame.Disable();
        _inputActions.InGame.SelectObject.performed -= SelectObject_performed;
    }

    private void Update()
    {
        Debug.Log(_objectValue); 
    }
}

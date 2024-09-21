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
    private Material _currentMaterial;
    private Material _previousMaterial;
    private Material _nextMaterial;
    private Material _currentChildMaterial;
    private Material _previousChildMaterial;
    private Material _nextChildMaterial;

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
        ResetMaterials();
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
        ResetMaterials();
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
        if (_interactObject.TryGetComponent(out ActivateSelected _scriptActivateSelected))
        {
            _scriptActivateSelected.Activate();
        }

        _currentMaterial = GetObjectMaterial(_interactObject);
        _currentChildMaterial = GetChildObjectMaterial(_interactObject);

        int previousIndex = (_objectValue - 1 + _interactObjectList.Count) % _interactObjectList.Count;
        int nextIndex = (_objectValue + 1) % _interactObjectList.Count;

        _previousMaterial = GetObjectMaterial(_interactObjectList[previousIndex]);
        _previousChildMaterial = GetChildObjectMaterial(_interactObjectList[previousIndex]);

        _nextMaterial = GetObjectMaterial(_interactObjectList[nextIndex]);
        _nextChildMaterial = GetChildObjectMaterial(_interactObjectList[nextIndex]);

        List<GameObject> activeObjects = new List<GameObject>
        {
            _interactObjectList[_objectValue],
            _interactObjectList[previousIndex],
            _interactObjectList[nextIndex]
        };

        foreach (GameObject obj in _interactObjectList)
        {
            if (!activeObjects.Contains(obj))
            {
                if (obj.TryGetComponent(out ActivateSelected _inactiveScript))
                {
                    DestroyObjectColor(obj);
                    _inactiveScript.Desactivate();
                }
            }
        }
    }

    private void OnDesactive(GameObject _interactObject)
    {
        if (_interactObject.TryGetComponent(out ActivateSelected _scriptActivateSelected))
        {
            _scriptActivateSelected.Desactivate();
        }
    }

    private Material GetObjectMaterial(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.material;
        }
        return null;
    }

    private Material GetChildObjectMaterial(GameObject obj)
    {
        if (obj.transform.childCount > 0)
        {
            GameObject child = obj.transform.GetChild(0).gameObject;
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                return childRenderer.material;
            }
        }
        return null;
    }

    public void ChangeCurrentObjectColor()
    {
        if (_currentMaterial != null)
        {
            _currentMaterial.SetFloat("_LeftValue", 0);
            _currentMaterial.SetFloat("_RightValue", 0);
            _currentMaterial.SetFloat("_MiddleValue", 1);
        }

        if (_currentChildMaterial != null)
        {
            _currentChildMaterial.SetFloat("_LeftValue", 0);
            _currentChildMaterial.SetFloat("_RightValue", 0);
            _currentChildMaterial.SetFloat("_MiddleValue", 1);
        }
    }

    public GameObject GetCurrentObject()
    {
        if (_objectValue >= 0 && _objectValue < _interactObjectList.Count)
        {
            return _interactObjectList[_objectValue];
        }
        return null;
    }

    public GameObject GetPreviousObject()
    {
        int previousIndex = (_objectValue - 1 + _interactObjectList.Count) % _interactObjectList.Count;
        return _interactObjectList[previousIndex];
    }

    public GameObject GetNextObject()
    {
        int nextIndex = (_objectValue + 1) % _interactObjectList.Count;
        return _interactObjectList[nextIndex];
    }

    public void ChangePreviousObjectColor()
    {
        if (_previousMaterial != null)
        {
            _previousMaterial.SetFloat("_LeftValue", 1);
        }

        if (_previousChildMaterial != null)
        {
            _previousChildMaterial.SetFloat("_RightValue", 0);
            _previousChildMaterial.SetFloat("_LeftValue", 1);
            _previousChildMaterial.SetFloat("_MiddleValue", 0);
        }
    }

    public void DestroyObjectColor(GameObject obj)
    {
        Material material = GetObjectMaterial(obj);
        Material childMaterial = GetChildObjectMaterial(obj);

        if (material != null)
        {
            material.SetFloat("_LeftValue", 0);
            material.SetFloat("_RightValue", 0);
            material.SetFloat("_MiddleValue", 0);
        }

        if (childMaterial != null)
        {
            childMaterial.SetFloat("_LeftValue", 0);
            childMaterial.SetFloat("_RightValue", 0);
            childMaterial.SetFloat("_MiddleValue", 0);
        }
    }

    private void ResetMaterials()
    {
        foreach (GameObject obj in _interactObjectList)
        {
            DestroyObjectColor(obj);
        }
    }

    public void ChangeNextObjectColor()
    {
        if (_nextMaterial != null)
        {
            _nextMaterial.SetFloat("_RightValue", 1);
        }

        if (_nextChildMaterial != null)
        {
            _nextChildMaterial.SetFloat("_LeftValue", 0);
            _nextChildMaterial.SetFloat("_RightValue", 1);
            _nextChildMaterial.SetFloat("_MiddleValue", 0);
        }
    }

    private void OnDisable()
    {
        _inputActions.InGame.Disable();
        _inputActions.InGame.SelectObject.performed -= SelectObject_performed;
    }

    private void Update()
    {
        ChangeCurrentObjectColor();
        ChangePreviousObjectColor();
        ChangeNextObjectColor();
    }
}

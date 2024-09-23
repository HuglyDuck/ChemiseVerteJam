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

    [SerializeField] private List<GameObject> _linkedObjects = new List<GameObject>();

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

    private void OnDisable()
    {
        _inputActions.InGame.Disable();
        _inputActions.InGame.SelectObject.performed -= SelectObject_performed;
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
        if (_interactObjectList.Count > 1) 
        {
            ResetMaterials();
            OnDesactive(_interactObjectList[_objectValue]);
            _objectValue = (_objectValue >= _interactObjectList.Count - 1) ? 0 : _objectValue + 1;
            OnActive(_interactObjectList[_objectValue]);
        }
        else
        {
            OnActive(_interactObjectList[_objectValue]);
        }
    }

    private void SelectObjectSub()
    {
        if (_interactObjectList.Count > 1)
        {
            ResetMaterials();
            OnDesactive(_interactObjectList[_objectValue]);
            _objectValue = (_objectValue <= 0) ? _interactObjectList.Count - 1 : _objectValue - 1;
            OnActive(_interactObjectList[_objectValue]);
        }
        else
        {

            OnActive(_interactObjectList[_objectValue]);
        }
    }

    private void OnActive(GameObject _interactObject)
    {
        if (_interactObject.TryGetComponent(out ActivateSelected _scriptActivateSelected))
        {
            _scriptActivateSelected.Activate();
        }

        _currentMaterial = GetObjectMaterial(_interactObject);
        _currentChildMaterial = GetChildObjectMaterial(_interactObject);

        if (_interactObjectList.Count == 1) 
        {
            _previousMaterial = null;
            _nextMaterial = null;
            _previousChildMaterial = null;
            _nextChildMaterial = null;
        }
        else 
        {
            int previousIndex = (_objectValue - 1 + _interactObjectList.Count) % _interactObjectList.Count;
            int nextIndex = (_objectValue + 1) % _interactObjectList.Count;

            _previousMaterial = GetObjectMaterial(_interactObjectList[previousIndex]);
            _previousChildMaterial = GetChildObjectMaterial(_interactObjectList[previousIndex]);

            _nextMaterial = GetObjectMaterial(_interactObjectList[nextIndex]);
            _nextChildMaterial = GetChildObjectMaterial(_interactObjectList[nextIndex]);
        }

        List<GameObject> activeObjects = new List<GameObject> { _interactObject };

        if (_interactObjectList.Count > 1)
        {
            activeObjects.Add(_interactObjectList[(_objectValue - 1 + _interactObjectList.Count) % _interactObjectList.Count]);
            activeObjects.Add(_interactObjectList[(_objectValue + 1) % _interactObjectList.Count]);
        }

        if (_currentMaterial == null && _currentChildMaterial == null)
        {
            ChangeLinkedObjectsColor(0, 0, 1); 
        }

        if (_previousMaterial == null && _previousChildMaterial == null && _interactObjectList.Count > 1)
        {
            ChangeLinkedObjectsColor(1, 0, 0); 
        }

        if (_nextMaterial == null && _nextChildMaterial == null && _interactObjectList.Count > 1)
        {
            ChangeLinkedObjectsColor(0, 1, 0); 
        }

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
        return renderer != null ? renderer.material : null;
    }

    private Material GetChildObjectMaterial(GameObject obj)
    {
        if (obj.transform.childCount > 0)
        {
            GameObject child = obj.transform.GetChild(0).gameObject;
            Renderer childRenderer = child.GetComponent<Renderer>();
            return childRenderer != null ? childRenderer.material : null;
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

    public void ChangePreviousObjectColor()
    {
        if (_previousMaterial != null)
        {
            _previousMaterial.SetFloat("_LeftValue", 1);
            _previousMaterial.SetFloat("_RightValue", 0);
            _previousMaterial.SetFloat("_MiddleValue", 0);
        }

        if (_previousChildMaterial != null)
        {
            _previousChildMaterial.SetFloat("_LeftValue", 1);
            _previousChildMaterial.SetFloat("_RightValue", 0);
            _previousChildMaterial.SetFloat("_MiddleValue", 0);
        }

        if (_previousMaterial == null && _previousChildMaterial == null && _interactObjectList.Count > 1)
        {
            ChangeLinkedObjectsColor(1, 0, 0); 
        }
    }

    public void ChangeNextObjectColor()
    {
        if (_nextMaterial != null)
        {
            _nextMaterial.SetFloat("_LeftValue", 0);
            _nextMaterial.SetFloat("_RightValue", 1);
            _nextMaterial.SetFloat("_MiddleValue", 0);
        }

        if (_nextChildMaterial != null)
        {
            _nextChildMaterial.SetFloat("_LeftValue", 0);
            _nextChildMaterial.SetFloat("_RightValue", 1);
            _nextChildMaterial.SetFloat("_MiddleValue", 0);
        }

        if (_nextMaterial == null && _nextChildMaterial == null && _interactObjectList.Count > 1)
        {
            ChangeLinkedObjectsColor(0, 1, 0);
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

    private void DestroyLinkedObjectsColor()
    {
        foreach (GameObject linkedObject in _linkedObjects)
        {
            DestroyObjectColor(linkedObject);
        }
    }

    private void ResetMaterials()
    {
        foreach (GameObject obj in _interactObjectList)
        {
            DestroyObjectColor(obj);
        }

        DestroyLinkedObjectsColor();
    }
    private void ChangeLinkedObjectsColor(float leftValue, float rightValue, float middleValue)
    {
        foreach (GameObject linkedObject in _linkedObjects)
        {
            Material linkedMaterial = GetObjectMaterial(linkedObject);
            Material linkedChildMaterial = GetChildObjectMaterial(linkedObject);

            if (linkedMaterial != null)
            {
                linkedMaterial.SetFloat("_LeftValue", leftValue);
                linkedMaterial.SetFloat("_RightValue", rightValue);
                linkedMaterial.SetFloat("_MiddleValue", middleValue);
            }

            if (linkedChildMaterial != null)
            {
                linkedChildMaterial.SetFloat("_LeftValue", leftValue);
                linkedChildMaterial.SetFloat("_RightValue", rightValue);
                linkedChildMaterial.SetFloat("_MiddleValue", middleValue);
            }
        }
    }


    private void Update()
    {
        ChangePreviousObjectColor();
        ChangeCurrentObjectColor();
        ChangeNextObjectColor();
    }
}

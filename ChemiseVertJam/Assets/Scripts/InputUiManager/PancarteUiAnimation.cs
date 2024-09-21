using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PancarteUiAnimation : MonoBehaviour
{
    private GameInputs _gameInput;
    private float _rblbValue;
    private float _currentTiltX;
    private float _tiltVelocity;
    [SerializeField] private float tiltRange = 30f;
    [SerializeField] private float smoothTime = 0.2f; 
    [SerializeField] private float resetDelay = 1f; 

    private void Awake()
    {
        _gameInput = new GameInputs();
    }

    private void OnEnable()
    {
        _gameInput.InGame.SelectObject.Enable();
        _gameInput.InGame.SelectObject.performed += SelectObject_performed;
        _gameInput.InGame.SideMove.canceled += OnAxisCancel;
    }

    private void OnDisable()
    {
        _gameInput.InGame.SelectObject.Disable();
        _gameInput.InGame.SelectObject.performed -= SelectObject_performed;
        _gameInput.InGame.SideMove.canceled -= OnAxisCancel;
    }

    private void SelectObject_performed(InputAction.CallbackContext context)
    {
        _rblbValue = context.ReadValue<float>();
        StopAllCoroutines();
        StartCoroutine(ResetValueAfterDelay());
    }

    private void OnAxisCancel(InputAction.CallbackContext context)
    {
        _rblbValue = 0f;
    }

    private void Update()
    {
        float targetTiltX = Mathf.Lerp(-tiltRange, tiltRange, (_rblbValue + 1) / 2f);
        _currentTiltX = Mathf.SmoothDamp(_currentTiltX, targetTiltX, ref _tiltVelocity, smoothTime);

        transform.localRotation = Quaternion.Euler(_currentTiltX, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }

    private IEnumerator ResetValueAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        _rblbValue = 0f;
    }
}

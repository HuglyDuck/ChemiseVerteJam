using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickLocalTiltX : MonoBehaviour
{
    private GameInputs _gameInput;
    private float _axisValue;
    [SerializeField] private float tiltRange = 30f;

    private void Awake()
    {
        _gameInput = new GameInputs();
    }

    private void OnEnable()
    {
        _gameInput.InGame.SideMove.Enable();
        _gameInput.InGame.SideMove.performed += OnAxisMove;
        _gameInput.InGame.SideMove.canceled += OnAxisCancel;
    }

    private void OnDisable()
    {
        _gameInput.InGame.SideMove.Disable();
        _gameInput.InGame.SideMove.performed -= OnAxisMove;
        _gameInput.InGame.SideMove.canceled -= OnAxisCancel;
    }

    private void OnAxisMove(InputAction.CallbackContext context)
    {
        _axisValue = context.ReadValue<float>();
    }

    private void OnAxisCancel(InputAction.CallbackContext context)
    {
        _axisValue = 0f;
    }

    private void Update()
    {
        float targetTiltX = Mathf.Lerp(-tiltRange, tiltRange, (_axisValue + 1) / 2f);

        // Appliquer la rotation sur l'axe X local en utilisant le point de pivot local
        transform.localRotation = Quaternion.Euler(targetTiltX, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInput : MonoBehaviour
{
    private GameInputs _gameInput;
    private float _axisValue;
    public float _aPushed;
    public float _bPushed;
    public float _xPushed;
    public float _rbPushed;
    public float _lbPushed;


    private void Awake()
    {
        _gameInput = new GameInputs();
    }

    private void OnEnable()
    {
        _gameInput.InGame.SideMove.Enable();
        _gameInput.InGame.SideMove.performed += OnAxisMove;
        _gameInput.InGame.SideMove.canceled += OnAxisCancel;
        _gameInput.InGame.StopPlayer.Enable();
        _gameInput.InGame.RunPlayer.Enable();
        _gameInput.InGame.RunPlayer.performed += RunPlayer_performed;
        _gameInput.InGame.StopPlayer.performed += StopPlayer_performed;
        _gameInput.InGame.SwitchLight.Enable();
        _gameInput.InGame.SwitchLight.performed += SwitchLight_performed;


    }

    private void OnDisable()
    {
        _gameInput.InGame.SideMove.Disable();
        _gameInput.InGame.SideMove.performed -= OnAxisMove;
        _gameInput.InGame.SideMove.canceled -= OnAxisCancel;
        _gameInput.InGame.StopPlayer.Enable();
        _gameInput.InGame.RunPlayer.Enable();
        _gameInput.InGame.RunPlayer.performed -= RunPlayer_performed;
        _gameInput.InGame.StopPlayer.performed -= StopPlayer_performed;
        _gameInput.InGame.SwitchLight.Enable();
        _gameInput.InGame.SwitchLight.performed -= SwitchLight_performed;
        _gameInput.InGame.SelectObject.Enable();
        _gameInput.InGame.SelectObject.performed -= SelectObject_performed;

    }

    private void OnAxisMove(InputAction.CallbackContext context)
    {
        _axisValue = context.ReadValue<float>();
    }

    private void OnAxisCancel(InputAction.CallbackContext context)
    {
        _axisValue = 0f;
    }
    private void RunPlayer_performed(InputAction.CallbackContext context)
    {
        _xPushed = 1f;
    }

    private void StopPlayer_performed(InputAction.CallbackContext context)
    {
        _bPushed = 1f;
    }
    private void SwitchLight_performed(InputAction.CallbackContext context)
    {
        _aPushed = 1f;
    }
    private void SelectObject_performed(InputAction.CallbackContext context)
    {
        _aPushed = 1f;
    }
    private void Update()
    {
        float moveX = _axisValue;
        if (_bPushed > 0f)
        {
            _bPushed -= Time.deltaTime * 5;
        }

        if (_xPushed > 0f)
        {
            _xPushed -= Time.deltaTime * 5;
        }

        if (_aPushed > 0f)
        {
            _aPushed -= Time.deltaTime * 5;
        }


    }
}

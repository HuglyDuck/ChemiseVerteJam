using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsSwitch : MonoBehaviour
{
    private GameInputs _inputs;
    [SerializeField] private GameObject _pauseMenu;

    private void Awake()
    {
        _inputs = new GameInputs();
    }
    private void OnEnable()
    {
        _inputs.InGame.PauseGame.Enable();
        _inputs.InGame.PauseGame.performed += OpenPauseMenu;
    }
    private void OnDisable()
    {
        _inputs.InGame.PauseGame.Disable();
        _inputs.InGame.PauseGame.performed -= OpenPauseMenu;
    }

    private void OpenPauseMenu(InputAction.CallbackContext ctx)
    {
        _pauseMenu.SetActive(true);
        if(_pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

}

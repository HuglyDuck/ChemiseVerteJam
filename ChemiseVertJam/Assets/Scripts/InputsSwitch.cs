using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputsSwitch : MonoBehaviour
{
    private GameInputs _inputs;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _popUpPanel;
    [SerializeField] private GameObject _endingScreen;
    [SerializeField] private Button _resumeButton;

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
        if (_settingsMenu.activeSelf || (_popUpPanel != null && _popUpPanel.activeSelf) || _endingScreen.activeSelf)
        {
            return;
        }

        if (_pauseMenu.activeSelf)
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
            _resumeButton.Select();
        }
    }


}

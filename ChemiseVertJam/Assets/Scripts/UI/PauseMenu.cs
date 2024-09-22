using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private GameObject _pauseMenuPanel;

    [SerializeField] private Button _playButton;

    [SerializeField] private int _mainMenuSceneIndex = 0;

    private GameInputs _gameInputs;



    private void Awake()
    {
        _gameInputs = new GameInputs();
    }
    //private void OnEnable()
    //{
    //    _playButton.Select();
    //    _gameInputs.UI.Cancel.Enable();
    //    _gameInputs.UI.Cancel.performed += CloseAllPanels;
    //}
    //private void OnDisable()
    //{
    //    _gameInputs.UI.Cancel.Disable();
    //    _gameInputs.UI.Cancel.performed -= CloseAllPanels;
    //}

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        SwitchPauseMenu(false);
        _soundSlider.Select();
    }
    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        SwitchPauseMenu(true);
        _playButton.Select();
    }
    public void GoToMainMenu()
    {

        SceneManager.LoadScene(_mainMenuSceneIndex);
        Time.timeScale = 1.0f;
    }

    private void CloseAllPanels(InputAction.CallbackContext ctx)
    {
        _settingsPanel.SetActive(false);
        _pauseMenuPanel.SetActive(true);
    }
    private void SwitchPauseMenu(bool trigger)
    {
        _pauseMenuPanel.SetActive(trigger);
    }
}

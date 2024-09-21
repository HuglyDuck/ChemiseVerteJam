using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject _mainMenuPanel;
    private GameInputs _gameInputs;

    [Header("Select Level")]
    [SerializeField] private GameObject _selectLevelPanel;
    [SerializeField] private Button _levelOneButton;
    [SerializeField] private Button _playGameButton;

    [Header("Settings")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Slider _soundSlider;

    [Header("Credits")]
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _closeCreditsButton;

    private void Awake()
    {
        _gameInputs = new GameInputs();
    }
    private void OnEnable()
    {
        _gameInputs.UI.Cancel.Enable();
        _gameInputs.UI.Cancel.performed += CloseAllPanels;
    }
    private void OnDisable()
    {
        _gameInputs.UI.Cancel.Disable();
        _gameInputs.UI.Cancel.performed -= CloseAllPanels;
    }
    public void OpenLevelsSelection()
    {
        _selectLevelPanel.SetActive(true);
        SwitchMainMenu(false);
        _levelOneButton.Select();
    }
    public void CloseLevelsSelection()
    {
        _selectLevelPanel.SetActive(false);
        SwitchMainMenu(true);
        _playGameButton.Select();
    }

    private void CloseAllPanels(InputAction.CallbackContext ctx)
    {
        _creditsPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _selectLevelPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        _playGameButton.Select();
    }
    public void PlayLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenSettings()
    {
        SwitchMainMenu(false);
        _settingsPanel.SetActive(true);
        _soundSlider.Select();
    }
    public void CloseSettings()
    {
        SwitchMainMenu(true);
        _settingsPanel.SetActive(false);
        _settingsButton.Select();
    }

    public void OpenCredits()
    {
        SwitchMainMenu(false);
        _creditsPanel.SetActive(true);
        _closeCreditsButton.Select();
    }
    public void CloseCredits()
    {
        SwitchMainMenu(true);
        _creditsPanel.SetActive(false);
        _creditsButton.Select();
    }

    private void SwitchMainMenu(bool trigger)
    {
        _mainMenuPanel.SetActive(trigger);
    }
}

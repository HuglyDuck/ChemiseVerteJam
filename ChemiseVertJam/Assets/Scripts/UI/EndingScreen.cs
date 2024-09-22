using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private Button _nextLevelButton;

    public static EndingScreen Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.N))
        {
            ShowVictoryScreen();
            _nextLevelButton.Select();
        }
    }

    public void ShowVictoryScreen()
    {
        _victoryPanel.SetActive(true);
        Time.timeScale = 0.0f;
        _nextLevelButton.Select();
    }


    public void RestartGame()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(currentScene);
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(nextSceneIndex);
    }


}

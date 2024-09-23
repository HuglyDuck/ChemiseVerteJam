using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPopUp;
    [SerializeField] private Button _backButton;


    private void Start()
    {
        _tutorialPopUp.SetActive(true);
        Time.timeScale = 0.0f;
        _backButton.Select();
        GamepadRumble.Instance.StopRumble();
    }

    public void CloseTutorialPopUp()
    {
        _tutorialPopUp.SetActive(false);
        Time.timeScale = 1.0f;
    }
}

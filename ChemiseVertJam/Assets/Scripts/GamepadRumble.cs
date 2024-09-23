using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadRumble : MonoBehaviour
{
    public static GamepadRumble Instance { get; private set; }

    private bool _isRumbling = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartRumble(float strength)
    {
        if (!_isRumbling)
        {
            _isRumbling = true;
            var gamepad = GetActiveGamepad();
            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(strength, strength);
            }
        }
    }

    public void StopRumble()
    {
        _isRumbling = false;
        var gamepad = GetActiveGamepad();
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0, 0);
        }
    }

    public void UpdateRumble(float strength)
    {
        if (_isRumbling)
        {
            var gamepad = GetActiveGamepad();
            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(strength, strength);
            }
        }
    }

    private Gamepad GetActiveGamepad()
    {
        return Gamepad.all[0]; 
    }
    private void OnDisable()
    {
        StopRumble();
    }
}

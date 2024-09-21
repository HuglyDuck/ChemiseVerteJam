using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightDown : MonoBehaviour
{
    private GameInputs _inputActions;
    public List<GameObject> _lights = new List<GameObject>();
    [SerializeField] private float _timer;
    [SerializeField] private ResetLightDown _scriptResetLightDown;
    private float _timeValue;

    private void Awake()
    {
        _inputActions = new GameInputs();
    }



    private void OnEnable()
    {
        _inputActions.InGame.SwitchLight.Enable();
        _inputActions.InGame.SwitchLight.performed += SwitchLight_performed;
    }

    private void SwitchLight_performed(InputAction.CallbackContext context)
    {
        
        if (!_scriptResetLightDown._enabled)
        {
            Debug.Log("test");
            foreach (GameObject go in _lights)
            {
                go.SetActive(false);
            }

            _scriptResetLightDown.TimerReset(_timer);
        }
    }

 

    private void OnDisable()
    {
        _inputActions.InGame.SwitchLight.Disable();
        _inputActions.InGame.SwitchLight.performed -= SwitchLight_performed;
    }

}

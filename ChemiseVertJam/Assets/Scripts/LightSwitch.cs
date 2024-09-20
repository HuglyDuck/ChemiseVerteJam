using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class LightSwitch : MonoBehaviour
{
    private GameInputs _inputActions;

    [SerializeField] private List<GameObject> _lightOn = new List<GameObject>();
    [SerializeField] private List<GameObject> _lightOff = new List<GameObject>();

    public bool _switch;

    private void Awake()
    {
        _inputActions = new GameInputs();
        SwitchLightOff();
    }

    private void OnEnable()
    {
        _inputActions.InGame.SwitchLight.Enable();
        _inputActions.InGame.SwitchLight.performed += SwitchLight_performed;
    }

    private void SwitchLight_performed(InputAction.CallbackContext context)
    {
        if (_switch) SwitchLightOn();
        else SwitchLightOff();


    }

    private void OnDisable()
    {
        _inputActions.InGame.SwitchLight.Disable();
        _inputActions.InGame.SwitchLight.performed -= SwitchLight_performed;
    }

    private void SwitchLightOn()
    {
        _switch = false;
        foreach (GameObject go in _lightOn)
        {
            go.SetActive(true);
        }

        foreach (GameObject go in _lightOff)
        {
            go.SetActive(false);
        }
    }

    private void SwitchLightOff()
    {
        _switch = true;
        foreach (GameObject go in _lightOn)
        {
            go.SetActive(false);
        }

        foreach (GameObject go in _lightOff)
        {
            go.SetActive(true);
        }
    }

}

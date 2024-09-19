using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class LightSwitch : MonoBehaviour
{
    GameInputs m_inputActions;

<<<<<<< Updated upstream
    //[SerializeField] List<GameObject> m_
=======
>>>>>>> Stashed changes

    private void Awake()
    {
        m_inputActions = new GameInputs();
    }

    private void OnEnable()
    {
        m_inputActions.InGame.SwitchLight.Enable();
        m_inputActions.InGame.SwitchLight.performed += SwitchLight_performed;
    }

    private void SwitchLight_performed(InputAction.CallbackContext context)
    {
        
    }

    private void OnDisable()
    {
        m_inputActions.InGame.SwitchLight.Disable();
        m_inputActions.InGame.SwitchLight.performed -= SwitchLight_performed;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}

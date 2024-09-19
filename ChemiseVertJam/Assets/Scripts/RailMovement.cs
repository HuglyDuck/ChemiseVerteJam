using UnityEngine;
using UnityEngine.InputSystem;
public class ObjectMovement : MonoBehaviour
{
    
    public float _speed = 5f;         
    public float _leftLimit = -5f;    
    public float _rightLimit = 5f;    
    public Color _gizmoColor = Color.red;
    private float _input;   
    private float _currentXPosition;  
    private GameInputs _gameInput;

    private void Awake()
    {
        _gameInput = new GameInputs();
    }

    private void OnEnable()
    {
        _gameInput.InGame.SideMove.Enable();
        _gameInput.InGame.SideMove.performed += OnSideMovement;
        _gameInput.InGame.SideMove.canceled += ResetInput;
    }
    private void OnDisable()
    {
        _gameInput.InGame.SideMove.Disable();
        _gameInput.InGame.SideMove.performed -= OnSideMovement;
        _gameInput.InGame.SideMove.canceled -= ResetInput;
    }
    public void ResetInput(InputAction.CallbackContext context)
    {
        _input = 0;
    }
    public void OnSideMovement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<float>();
    }
    void Update()
    {
      
        if(_input != 0)
        {
            UpdateMovement();
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = _gizmoColor;
  
        Gizmos.DrawLine(new Vector3(_leftLimit, transform.position.y - 1, transform.position.z),
                        new Vector3(_leftLimit, transform.position.y + 1, transform.position.z));
    
        Gizmos.DrawLine(new Vector3(_rightLimit, transform.position.y - 1, transform.position.z),
                        new Vector3(_rightLimit, transform.position.y + 1, transform.position.z));
     
        Gizmos.DrawLine(new Vector3(_leftLimit, transform.position.y, transform.position.z),
                        new Vector3(_rightLimit, transform.position.y, transform.position.z));
    }

    private void UpdateMovement()
    {
        _currentXPosition = transform.position.x;

        float movement = _input  * _speed * Time.deltaTime;

        float newPositionX = _currentXPosition + movement;

        newPositionX = Mathf.Clamp(newPositionX, _leftLimit, _rightLimit);

        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
    }
}
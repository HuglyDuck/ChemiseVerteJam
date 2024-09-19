using UnityEngine;
using UnityEngine.InputSystem;
public class ObjectMovement : MonoBehaviour
{
    #region VARIABLES
    [Header("Rail Parameters")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _leftLimit = -5f;
    [SerializeField] private float _rightLimit = 5f;
    [SerializeField] private Color _gizmoColor = Color.red;


    private float _input;
    private float _currentXPosition;
    private GameInputs _gameInput;
    #endregion

    #region INPUTS
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
    #endregion
    void Update()
    {


        UpdateMovement();

    }

    #region GIZMOS
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
    #endregion

    #region MOVEMENT
    private void UpdateMovement()
    {
        _currentXPosition = transform.position.x;

        float movement = _input * _speed * Time.deltaTime;

        float newPositionX = _currentXPosition + movement;

        newPositionX = Mathf.Clamp(newPositionX, _leftLimit, _rightLimit);

        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
    }
}
#endregion
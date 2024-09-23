using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectMovement : MonoBehaviour
{
    #region VARIABLES
    [Header("Rail Parameters")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _leftLimitObject;
    [SerializeField] private Transform _rightLimitObject;
    [SerializeField][Range(0, 1)] private float _spawnpoint = 0f;
    [SerializeField] private Color _gizmoColor = Color.red;

    [Header("Sound Parameters")]
    [SerializeField] private AudioSource _movementAudioSource; 

    private float _input;
    private GameInputs _gameInput;
    private Vector3 _movementDirection;
    private float _pathLength;
    private Vector3 _startPosition;
    private bool _isMoving;
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
        StopMovementSound();  
    }

    public void OnSideMovement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<float>();
        if (!_isMoving) StartMovementSound(); 
    }
    #endregion

    private void Start()
    {
        if (_leftLimitObject && _rightLimitObject)
        {
            _leftLimitObject.parent = null;
            _rightLimitObject.parent = null;

            _movementDirection = (_rightLimitObject.position - _leftLimitObject.position).normalized;
            _pathLength = Vector3.Distance(_leftLimitObject.position, _rightLimitObject.position);

            _startPosition = _leftLimitObject.position + _movementDirection * _spawnpoint * _pathLength;
            transform.position = _startPosition;
        }
    }

    private void Update()
    {
        if (_leftLimitObject && _rightLimitObject)
        {
            UpdateMovement();
        }

   
        float targetRotationZ = Mathf.Lerp(-15f, 15f, (_input + 1f) / 2f);
        transform.rotation = Quaternion.Euler(0f, 0f, targetRotationZ);

 
        HandleMovementSound();
    }

    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (_leftLimitObject && _rightLimitObject)
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawLine(_leftLimitObject.position, _rightLimitObject.position);
            Gizmos.DrawSphere(_leftLimitObject.position, 0.2f);
            Gizmos.DrawSphere(_rightLimitObject.position, 0.2f);

            if (Application.isPlaying)
            {
                Gizmos.DrawSphere(transform.position, 0.2f);
            }
            else
            {
                Vector3 spawnPosition = _leftLimitObject.position + (_rightLimitObject.position - _leftLimitObject.position) * _spawnpoint;
                Gizmos.DrawSphere(spawnPosition, 0.2f);
            }
        }
    }
    #endregion

    #region MOVEMENT
    private void UpdateMovement()
    {
        Vector3 currentPosition = transform.position;
        Vector3 projectedPosition = Vector3.Project(currentPosition - _leftLimitObject.position, _movementDirection) + _leftLimitObject.position;

        float movement = _input * _speed * Time.deltaTime;
        projectedPosition += _movementDirection * movement;

        float distanceFromLeft = Vector3.Distance(projectedPosition, _leftLimitObject.position);
        float distanceFromRight = Vector3.Distance(projectedPosition, _rightLimitObject.position);

        if (distanceFromLeft > _pathLength)
        {
            projectedPosition = _rightLimitObject.position;
        }
        else if (distanceFromRight > _pathLength)
        {
            projectedPosition = _leftLimitObject.position;
        }

        transform.position = projectedPosition;
    }
    #endregion

    #region SOUND
    private void StartMovementSound()
    {
        if (_movementAudioSource && !_movementAudioSource.isPlaying)
        {
            _movementAudioSource.Play();
            _isMoving = true;
        }
    }

    private void StopMovementSound()
    {
        if (_movementAudioSource && _movementAudioSource.isPlaying)
        {
            _movementAudioSource.Stop();
            _isMoving = false;
        }
    }

    private void HandleMovementSound()
    {
        if (_input == 0 && _isMoving)
        {
            StopMovementSound();
        }
        else if (_input != 0 && !_isMoving)
        {
            StartMovementSound();
        }
    }
    #endregion
}

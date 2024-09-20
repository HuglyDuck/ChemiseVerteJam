using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;

public class SplineMovement : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    float distancePercentage = 0f;

    [SerializeField] private float _runSpeed = 4f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _moveTowardSpeed = 1f;
    private float _targetSpeed;
    private float _currentSpeed;
    private int _speedIndex = 0;
    private GameInputs _inputActions;

    float splineLength;

    private void Awake()
    {
        _inputActions = new GameInputs();
    }

    private void OnEnable()
    {
        _inputActions.InGame.StopPlayer.Enable();
        _inputActions.InGame.RunPlayer.Enable();
        _inputActions.InGame.RunPlayer.performed += RunPlayer_performed;
        _inputActions.InGame.StopPlayer.performed += StopPlayer_performed;
    }

    private void RunPlayer_performed(InputAction.CallbackContext context)
    {
        if(_speedIndex == 2)
        {
            _speedIndex = 1;
            _targetSpeed = _speed;
        }
        else
        {
            _speedIndex = 2;
            _targetSpeed = _runSpeed;
        }
    }

    private void StopPlayer_performed(InputAction.CallbackContext context)
    {
        if (_speedIndex == 0)
        {
            _speedIndex = 1;
            _targetSpeed = _speed;
        }
        else
        {
            _speedIndex = 0;
            _targetSpeed = 0;
        }
    }

    private void Start()
    {
        splineLength = spline.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, _moveTowardSpeed * Time.deltaTime);

        distancePercentage += _currentSpeed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    private void OnDisable()
    {
        _inputActions.InGame.StopPlayer.Enable();
        _inputActions.InGame.RunPlayer.Enable();
    }
}

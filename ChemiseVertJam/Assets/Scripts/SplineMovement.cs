using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;

public class SplineMovement : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speed = 1f;
    float distancePercentage = 0f;

    [SerializeField] private float _runSpeed;
    [SerializeField] private float _speed;
    private float _targetSpeed;
    private float _currentSpeed;
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
        if(_currentSpeed >= _runSpeed)
        {

        }
    }

    private void StopPlayer_performed(InputAction.CallbackContext context)
    {

    }

    private void Start()
    {
        splineLength = spline.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {
        distancePercentage += speed * Time.deltaTime / splineLength;

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

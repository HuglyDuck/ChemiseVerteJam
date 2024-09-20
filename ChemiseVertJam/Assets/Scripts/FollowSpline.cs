using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class FollowSpline : MonoBehaviour
{
    #region VARIABLES
    [Header("References")]
    [SerializeField] private List<ProceduralBezierCurve> _splines;
    [SerializeField] private bool _isPlayer;

    private int _currentSplineIndex = 0;
    private float _splineProgress = 0f;
    private Vector3 _previousPosition;
    private GameInputs _gameInputs;
    private bool _canMove = true;
    #endregion

    private void Awake()
    {
        _gameInputs = new GameInputs();
    }
    private void OnEnable()
    {
        _gameInputs.InGame.StopPlayer.Enable();
        _gameInputs.InGame.StopPlayer.performed += StopMovement;
    }
    private void OnDisable()
    {
        _gameInputs.InGame.StopPlayer.Disable();
        _gameInputs.InGame.StopPlayer.performed -= StopMovement;
    }
    void Start()
    {
        if (_splines == null || _splines.Count == 0)
        {
            Debug.LogError("forgot to reference spline");
            return;
        }

        UpdateSpline();
    }

    void Update()
    {
        if (_canMove)
        {
            MoveAlongSpline(); 
        }
    }

    #region MOVEMENT_LOGIC
    private void MoveAlongSpline()
    {
        if (_splines == null || _splines.Count == 0)
            return;

        var currentSpline = _splines[_currentSplineIndex];
        float distanceToMove;

        if (currentSpline.ConstantSpeed)
        {
            distanceToMove = currentSpline.SplineSpeed * Time.deltaTime;
        }
        else
        {
            distanceToMove = currentSpline.Curve.Evaluate(_splineProgress) * Time.deltaTime;
        }

        _splineProgress += distanceToMove / currentSpline.ControlPoints.Count;
        _splineProgress = Mathf.Clamp01(_splineProgress);

        Vector3 newPosition = ProceduralBezierCurve.GetPositionOnSpline(_splineProgress, currentSpline.ControlPoints);

        transform.position = newPosition;
        Vector3 moveDirection = (newPosition - _previousPosition).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        _previousPosition = newPosition;

        if (_splineProgress >= 1f)
        {
            _splineProgress = 0f;
            _currentSplineIndex++;

            if (_currentSplineIndex >= _splines.Count)
            {
                _currentSplineIndex = 0;
            }

            UpdateSpline();
        }
    }
    #endregion

    private void UpdateSpline()
    {
        if (_splines != null && _splines.Count > 0 && _splines[_currentSplineIndex] != null)
        {
            _previousPosition = ProceduralBezierCurve.GetPositionOnSpline(_splineProgress, _splines[_currentSplineIndex].ControlPoints);
            transform.position = _previousPosition;
        }
    }

    private void StopMovement(InputAction.CallbackContext ctx)
    {
        if (_isPlayer)
        {
            _canMove = !_canMove;
        }
    }
}

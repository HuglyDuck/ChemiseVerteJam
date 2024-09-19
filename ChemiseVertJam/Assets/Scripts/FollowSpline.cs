using UnityEngine;
using System.Collections.Generic;

public class FollowSpline : MonoBehaviour
{
    #region VARIABLES
    [Header("Movement Params")]
    [SerializeField] private float _speed = 5f;

    [Header("References")]
    [SerializeField] private List<ProceduralBezierCurve> _splines;

    private int _currentSplineIndex = 0;
    private float _splineProgress = 0f;
    private Vector3 _previousPosition; 
    #endregion

    void Start()
    {
        if (_splines == null || _splines.Count == 0)
        {
            Debug.Log("no splines assigned");
            return;
        }

        UpdateSpline();
    }

    void Update()
    {
        MoveAlongSpline();
    }
    #region MOVEMENT_LOGIC
    private void MoveAlongSpline()
    {
        if (_splines != null && _splines.Count > 0)
        {
            float distanceToMove = _speed * Time.deltaTime;
            _splineProgress += distanceToMove / _splines[_currentSplineIndex].ControlPoints.Count;
            _splineProgress = Mathf.Clamp01(_splineProgress);

            Vector3 newPosition = ProceduralBezierCurve.GetPositionOnSpline(_splineProgress, _splines[_currentSplineIndex].ControlPoints);

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
}

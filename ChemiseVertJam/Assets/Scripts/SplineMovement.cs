using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;
using System;

public class SplineMovement : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    public float distancePercentage {  get; private set; }
    [SerializeField] private bool _pingPong;

    public float _currentSpeed = 2f;

    private bool _directionSwitch = true;

    float splineLength;

    public event Action _EndSpline;

    

    private void Start()
    {
        
        splineLength = spline.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {
        if(_directionSwitch)
            distancePercentage += _currentSpeed * Time.deltaTime / splineLength;
        else
            distancePercentage -= _currentSpeed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 1f || distancePercentage < 0f)
        {
            if (_pingPong) _directionSwitch = !_directionSwitch;
            else distancePercentage = 0f;
            _EndSpline?.Invoke();
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    
}

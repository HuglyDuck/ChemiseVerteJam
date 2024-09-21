using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;
using System;

public class SplineMovement : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    float distancePercentage = 0f;
    [SerializeField] private bool _pingPong;

    [SerializeField] private bool _vfx;
    [SerializeField,Range(0,1)] private float _vfxOn;
    [SerializeField, Range(0, 1)] private float _vfxOff;
    [SerializeField] private TrailRenderer _trailRenderer;

    public float _currentSpeed = 2f;

    private bool _directionSwitch = true;

    float splineLength;

    

    private void Start()
    {
        _trailRenderer.emitting = false;
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

        if (_vfx && distancePercentage > _vfxOff) _trailRenderer.emitting = false;
        if (_vfx && distancePercentage > _vfxOn && distancePercentage < _vfxOn + 0.1f) _trailRenderer.emitting = true;


        if (distancePercentage > 1f || distancePercentage < 0f)
        {
            if (_pingPong) _directionSwitch = !_directionSwitch;
            else distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxSpline : MonoBehaviour
{
    [SerializeField] private SplineMovement _scriptSplineMovement;
    [SerializeField, Range(0, 1)] private float _vfxOn;
    [SerializeField, Range(0, 1)] private float _vfxOff;
    [SerializeField] private TrailRenderer _trailRenderer;

    private void Start()
    {
        _trailRenderer.emitting = false;
    }
    private void Update()
    {
        if (_scriptSplineMovement.distancePercentage > _vfxOff) _trailRenderer.emitting = false;
        if (_scriptSplineMovement.distancePercentage > _vfxOn && _scriptSplineMovement.distancePercentage < _vfxOn + 0.1f) _trailRenderer.emitting = true;
    }
}

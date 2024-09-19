using UnityEngine;
using System.Collections.Generic;

public class ProceduralBezierCurve : MonoBehaviour
{
    #region VARIABLES
    [Header("Spline Params")]
    [SerializeField] private List<Transform> _controlPoints;
    [SerializeField] private float _splineSpeed;

    [Header("Debug")]
    [Range(0, 1)] public float _splineProgress = 0f; 
    [SerializeField][Range(1, 100)] private int _splineResolution = 20;
    public AnimationCurve _curve;
    #endregion

    public List<Transform> ControlPoints => _controlPoints;
    public float SplineSpeed => _splineSpeed;

    #region SPLINE_UTILITIES
    public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    public static Vector3 GetPositionOnSpline(float t, List<Transform> controlPoints)
    {
        if (controlPoints.Count < 4)
        {
            Debug.Log("not enough control points");
            return Vector3.zero;
        }

        int numSegments = (controlPoints.Count - 1) / 3;
        float segmentT = t * numSegments;
        int currentSegment = Mathf.Min(Mathf.FloorToInt(segmentT), numSegments - 1);
        float segmentLocalT = segmentT - currentSegment;

        Vector3 p0 = controlPoints[currentSegment * 3].position;
        Vector3 p1 = controlPoints[currentSegment * 3 + 1].position;
        Vector3 p2 = controlPoints[currentSegment * 3 + 2].position;
        Vector3 p3 = controlPoints[currentSegment * 3 + 3].position;

        return CalculateBezierPoint(segmentLocalT, p0, p1, p2, p3);
    } 
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (_controlPoints == null || _controlPoints.Count < 4)
            return;

        Gizmos.color = Color.blue;

        Vector3 previousPoint = _controlPoints[0].position;
        for (float i = 0; i <= 1; i += 1f / _splineResolution)
        {
            Vector3 currentPoint = GetPositionOnSpline(i, _controlPoints);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }



        Gizmos.color = Color.green;
        Vector3 tPoint = GetPositionOnSpline(_splineProgress, _controlPoints);
        Gizmos.DrawSphere(tPoint, 0.3f);
    } 
    #endregion
}

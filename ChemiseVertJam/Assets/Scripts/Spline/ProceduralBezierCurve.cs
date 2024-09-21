using UnityEngine;
using System.Collections.Generic;

public class ProceduralBezierCurve : MonoBehaviour
{
    #region VARIABLES
    [Header("Spline Params")]
    [SerializeField] private List<Transform> _controlPoints;

    [Header("Settings")]
    [SerializeField] private bool _constantSpeed = true;
    [SerializeField] private float _splineSpeed = 1f;

    [Header("Debug")]
    [SerializeField][Range(0, 1)] private float _splineProgress = 0f;
    [SerializeField][Range(1, 100)] private int _splineResolution = 20;
    #endregion

    public List<Transform> ControlPoints => _controlPoints;
    public float SplineSpeed => _splineSpeed;

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
            Debug.LogError("Need at least 4 control points for a Bézier curve.");
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

    public float TangentMagnitude(float t)
    {
        Vector3 tangent = Vector3.zero;
        int numSegments = (_controlPoints.Count - 1) / 3;
        float segmentT = t * numSegments;
        int currentSegment = Mathf.Min(Mathf.FloorToInt(segmentT), numSegments - 1);
        float segmentLocalT = segmentT - currentSegment;

        Vector3 p0 = _controlPoints[currentSegment * 3].position;
        Vector3 p1 = _controlPoints[currentSegment * 3 + 1].position;
        Vector3 p2 = _controlPoints[currentSegment * 3 + 2].position;
        Vector3 p3 = _controlPoints[currentSegment * 3 + 3].position;

        tangent += -3 * Mathf.Pow(1 - segmentLocalT, 2) * p0;
        tangent += 3 * Mathf.Pow(1 - segmentLocalT, 2) * p1;
        tangent += -6 * (1 - segmentLocalT) * segmentLocalT * p1;
        tangent += 6 * (1 - segmentLocalT) * segmentLocalT * p2;
        tangent += -3 * Mathf.Pow(segmentLocalT, 2) * p2;
        tangent += 3 * Mathf.Pow(segmentLocalT, 2) * p3;

        return tangent.magnitude;
    }

    // Calcul de la longueur d'arc
    public float ArcLength(float t)
    {
        return Integrate(x => TangentMagnitude(x), 0, t);
    }

    public float Parameter(float length)
    {
        float t = length / ArcLength(1);
        float lowerBound = 0f;
        float upperBound = 1f;

        for (int i = 0; i < 100; ++i)
        {
            float f = ArcLength(t) - length;

            if (Mathf.Abs(f) < 0.01f)
                break;

            float derivative = TangentMagnitude(t);
            float candidateT = t - f / derivative;

            if (f > 0)
            {
                upperBound = t;
                t = candidateT <= 0 ? (upperBound + lowerBound) / 2 : candidateT;
            }
            else
            {
                lowerBound = t;
                t = candidateT >= 1 ? (upperBound + lowerBound) / 2 : candidateT;
            }
        }

        return t;
    }

    public float Integrate(System.Func<float, float> func, float a, float b, int steps = 100)
    {
        float step = (b - a) / steps;
        float integral = 0f;

        for (int i = 0; i <= steps; i++)
        {
            float t = a + i * step;
            integral += func(t) * step;
        }

        return integral;
    }
}

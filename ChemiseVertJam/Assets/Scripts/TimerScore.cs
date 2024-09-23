using System;
using UnityEngine;

public class TimerScore : MonoBehaviour
{
    public static TimerScore Instance { get; private set; }

    private float _timePassed;
    private bool _isLevelFinished = false;
    private int _score;
    [SerializeField] private SplineMovement _movement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _movement._EndSpline += FinishLevel;
    }

    private void OnDisable()
    {
        _movement._EndSpline -= FinishLevel;
    }

    private void Update()
    {
        if (!_isLevelFinished)
        {
            _timePassed += Time.deltaTime;
        }
    }

    private void FinishLevel()
    {
        _isLevelFinished = true;
        CalculateScore();
        Debug.Log("time taken: " + _timePassed + " seconds. score: " + _score);
    }

    private void CalculateScore()
    {
        _score = Mathf.Max(0, 10000 - Mathf.FloorToInt(_timePassed * 100)); 
        _score = Mathf.Clamp(_score, 0, int.MaxValue); 
    }

    public float GetElapsedTime()
    {
        return _timePassed;
    }

    public int GetScore()
    {
        return _score;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollCurtains : MonoBehaviour
{
    public static ScrollCurtains Instance { get; private set; }

    [SerializeField] private float _topLimit = 5f;
    [SerializeField] private float _bottomLimit = -5f;
    [SerializeField] private AnimationCurve _scrollDownCurve;
    [SerializeField] private AnimationCurve _scrollUpCurve;
    [SerializeField] private bool _startAtBottom = false;
    [SerializeField] private SplineMovement _movement;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private Button _nextLevelButton;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _isScrolling = false;
    private float _scrollTime = 0f;
    private float _scrollDuration;
    private bool _restartOnBottom = false;

    
    private float _tolerance = 0.01f;

    [SerializeField] private float _restartTimeout = 1f; 
    private float _restartTimer = 0f;

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
        _movement._EndSpline += PrepareNextLevel;
        DetectPlayer.OnPlayerDied += HandleEndSpline;
    }

    private void OnDisable()
    {
        _movement._EndSpline -= PrepareNextLevel;
        DetectPlayer.OnPlayerDied -= HandleEndSpline;
    }

    private void Start()
    {
        if (_startAtBottom)
        {
            transform.position = new Vector3(transform.position.x, _bottomLimit, transform.position.z);
        }

        _startPosition = transform.position;
        _targetPosition = transform.position;

        ScrollUp();
    }

    private void Update()
    {
        UpdateScroll();

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    ScrollDown();
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ScrollUp();
        //}

       
        if (_restartOnBottom && Mathf.Abs(transform.position.y - _bottomLimit) <= _tolerance)
        {
            _restartTimer += Time.deltaTime;
            if (_restartTimer >= _restartTimeout)
            {
                ForceRestart();
            }
        }
    }

    private void UpdateScroll()
    {
        if (_isScrolling)
        {
            _scrollTime += Time.deltaTime;

            float normalizedTime = _scrollTime / _scrollDuration;
            AnimationCurve currentCurve = _targetPosition.y == _topLimit ? _scrollUpCurve : _scrollDownCurve;

            float curveValue = currentCurve.Evaluate(normalizedTime);
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, curveValue);

            if (normalizedTime >= 1f)
            {
                _isScrolling = false;

                
                if (Mathf.Abs(_targetPosition.y - _bottomLimit) <= _tolerance && _restartOnBottom)
                {
                    _restartTimer = 0f;
                }
            }
        }
    }
    private void PrepareNextLevel()
    {
        ScrollDown();  
        _restartOnBottom = false; 
        StartCoroutine(EnableEndScreenWhenDown());
    }

    private IEnumerator EnableEndScreenWhenDown()
    {
        while (_isScrolling)
        {
            yield return null; 
        }

        if (Mathf.Abs(transform.position.y - _bottomLimit) <= _tolerance)
        {
            _endScreen.SetActive(true);
            Time.timeScale = 0;
            _nextLevelButton.Select();
        }
    }
    private void HandleEndSpline()
    {
        _restartOnBottom = true;
        ScrollDown();
    }

    public void ScrollDown()
    {
        StartScrolling(_bottomLimit, _scrollDownCurve);
        GamepadRumble.Instance.StopRumble();
    }

    public void ScrollUp()
    {
        StartScrolling(_topLimit, _scrollUpCurve);
    }

    private void StartScrolling(float targetY, AnimationCurve curve)
    {
        _startPosition = transform.position;
        _targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        _scrollTime = 0f;
        _scrollDuration = curve.keys[curve.length - 1].time;
        _isScrolling = true;
    }

    private void ForceRestart()
    {
        if (_restartOnBottom)
        {
            Debug.Log("Restart Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 topPosition = new Vector3(transform.position.x, _topLimit, transform.position.z);
        Vector3 bottomPosition = new Vector3(transform.position.x, _bottomLimit, transform.position.z);

        Gizmos.DrawLine(topPosition, bottomPosition);
        Gizmos.DrawSphere(topPosition, 0.2f);
        Gizmos.DrawSphere(bottomPosition, 0.2f);
    }
}

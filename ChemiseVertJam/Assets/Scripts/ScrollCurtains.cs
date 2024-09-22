using UnityEngine;

public class ScrollCurtains : MonoBehaviour
{
    public static ScrollCurtains Instance { get; private set; }

    [SerializeField] private float _topLimit = 5f;
    [SerializeField] private float _bottomLimit = -5f;
    [SerializeField] private AnimationCurve _scrollDownCurve;
    [SerializeField] private AnimationCurve _scrollUpCurve;
    [SerializeField] private bool _startAtBottom = false;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _isScrolling = false;
    private float _scrollTime = 0f;
    private float _scrollDuration;

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

    private void Start()
    {
        if (_startAtBottom)
        {
            transform.position = new Vector3(transform.position.x, _bottomLimit, transform.position.z);
        }

        _startPosition = transform.position;
        _targetPosition = transform.position;
    }

    private void Update()
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
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ScrollDown();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ScrollUp();
        }
    }

    public void ScrollDown()
    {
        StartScrolling(_bottomLimit, _scrollDownCurve);
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

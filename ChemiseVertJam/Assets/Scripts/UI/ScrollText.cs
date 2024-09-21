using UnityEngine;

public class ScrollText : MonoBehaviour
{
    [SerializeField] private RectTransform _creditsText; 
    [SerializeField] private float _scrollSpeed = 50f;
    private Vector2 _originalPos;

    private void Start()
    {
        _originalPos = _creditsText.anchoredPosition;
    }
    private void Update()
    {
        _creditsText.anchoredPosition -= new Vector2(0, _scrollSpeed * Time.deltaTime);
    }
    private void OnDisable()
    {
        _creditsText.anchoredPosition = _originalPos;
    }
}

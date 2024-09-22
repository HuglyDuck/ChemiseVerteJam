using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    private float _alpha = 1f;
    private float _gradient = 0.01f;
    [SerializeField] private float _GradientMax = 1f;  
    [SerializeField] private float _GradientMin = 0f;  
    [SerializeField] private float _gradientSpeed = 1f;
    public DetectionManager detectionManager;

    private Material _material;

    void Start()
    {
        _gradient = _GradientMin;
        if (_gameObject != null)
        {
            Renderer renderer = _gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = renderer.material;
            }
        }
    }

    void Update()
    {


        _material.SetFloat("_Gradient", _gradient);
        _material.SetFloat("_Alpha", _alpha);


        if (_gradient > _GradientMin)
        {
            _alpha = 1f;
        }
        else
        {
            _alpha -= Time.deltaTime;
        }

        if (detectionManager.timer <= 0)
        {
            Undetected();
        }
        else
        {

            OnDetected();
        }
    }

    private void OnDetected()
    {
        float timeRemaining = Mathf.Clamp01(detectionManager.timer / detectionManager.timerDeath);
        _gradient = Mathf.Lerp(_GradientMin, _GradientMax, timeRemaining);
    }

    private void Undetected()
    {
        
        _gradient = Mathf.MoveTowards(_gradient, _GradientMin, _gradientSpeed * Time.deltaTime);
    }
}

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
    private Camera _mainCamera;

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

        // R�cup�rer la cam�ra principale
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // Si le GameObject est assign�, il regardera la cam�ra
        if (_gameObject != null && _mainCamera != null)
        {
            // Faire en sorte que l'objet regarde constamment vers la cam�ra
            _gameObject.transform.LookAt(_mainCamera.transform);

            // Inverser la rotation en Y pour que l'objet ne soit pas � l'envers
            _gameObject.transform.Rotate(0, 180, 0);
        }

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

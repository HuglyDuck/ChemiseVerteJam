using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Color _color;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Light _spotLight;
    private float _timer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected");
            _spotLight.color = _color;
            _timer += Time.deltaTime;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            _spotLight.color = _originalColor;
            _timer = 0f;
        }
    }
    private void Update()
    {
        Debug.Log(_timer);
    }
}

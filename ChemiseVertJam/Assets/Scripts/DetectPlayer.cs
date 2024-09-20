using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DetectPlayer : MonoBehaviour
{
    #region VARIABLES
    [Header("Detection Params")]
    [SerializeField] private Color _color;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Light _spotLight;
    [SerializeField] private LayerMask _layerToIgnore;
    private float _timer = 0f;
    private bool _inLight;
    private bool _inCircle;
    #endregion

    #region SPOT_ZONE
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _timer += Time.deltaTime;
            _inCircle = true;
            Debug.Log("In spot");


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _timer = 0f;
            _inCircle = false;
            _spotLight.color = _originalColor;
        }
    }
    #endregion

    private void Update()
    {
        CheckVisibility();
    }


    #region RAYCAST_LOGIC

    private void CheckVisibility()
    {
        foreach (var target in PlayerTargets.Instance.Targets)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToTarget, out hit, _spotLight.range, ~_layerToIgnore))
            {
                Debug.DrawLine(transform.position, hit.point, new Color(1, 0, 0));
                if (hit.collider.transform.CompareTag("Player"))
                {
                    _inLight = true;
                    Debug.Log("In ray");
                }
                else
                {
                    _inLight = false;
                }

            }
        }

        if (_inCircle && _inLight)
        {
            Debug.Log("Detected");
            _spotLight.color = _color;
        }
        
    }
    #endregion

}
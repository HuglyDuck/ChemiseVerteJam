using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
public class DetectPlayer : MonoBehaviour
{
    #region VARIABLES
    [Header("Detection Params")]
    [SerializeField] private Color _color;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Light _spotLight;
    [SerializeField] private LayerMask _layerToIgnore;
    [SerializeField] private float _timerDeath;
    private float _timer = 0f;
    private bool _inLight;
    private bool _inCircle;

    [SerializeField] private GameObject _targetObject;

    #endregion

    #region SPOT_ZONE
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        _inCircle = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        _timer = 0f;
    //        _inCircle = false;
    //        _spotLight.color = _originalColor;
    //    }
    //}


    #endregion

    private void Update()
    {
        CheckVisibility();
        if (IsInSpotlight(_targetObject))
        {
            Debug.Log(_targetObject.name + " In");
            Debug.Log("In Circle");
            _inCircle = true;
        }
        else
        {
            _inCircle = false;
            _spotLight.color = _originalColor;
        }
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
                    print("true");
                }
                else
                {
                    _inLight = false;
                    print("false");
                }

            }
        }

        if (_inCircle && _inLight)
        {
            Debug.Log("Detected");
            _spotLight.color = _color;
            _timer += Time.deltaTime;
            if(_timer >= _timerDeath)
            {
                Debug.Log("Player Died");
            }
        }
        else
        {
            _timer = 0;
        }
        
    }
    #endregion


    bool IsInSpotlight(GameObject obj)
    {
        if (obj == null) return false;


        Vector3 directionToTarget = obj.transform.position - transform.position;


        if (directionToTarget.magnitude > _spotLight.range)
        {
            return false;
        }


        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget > _spotLight.spotAngle / 2)
        {
            return false;
        }


        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, _spotLight.range))
        {

            if (hit.collider.gameObject == obj)
            {
                return true;
            }
        }

        return false;
    }
}
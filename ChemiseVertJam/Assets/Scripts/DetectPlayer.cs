using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [Header("Detection Params")]
    [SerializeField] private Color _color;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Light _spotLight;
    [SerializeField] private LayerMask _layerToIgnore;

    private bool _inLight;
    private bool _inCircle;
    private bool _isDetectingPlayer = false;  // Nouveau booléen pour suivre l'état de détection

    [SerializeField] private GameObject _targetObject;

    private void Update()
    {
        CheckVisibility();

        if (IsInSpotlight(_targetObject))
        {
            _inCircle = true;
        }
        else
        {
            _inCircle = false;
            _spotLight.color = _originalColor;
        }

        if (_inCircle && _inLight)
        {
            if (!_isDetectingPlayer)
            {
                DetectionManager.Instance.PlayerDetected();  // Signaler la détection au gestionnaire
                _isDetectingPlayer = true;
            }

            _spotLight.color = _color;
            DetectionManager.Instance.timer += Time.deltaTime;  // Augmenter le timer global

            if (DetectionManager.Instance.timer >= DetectionManager.Instance.timerDeath)
            {
                Debug.Log("Player Died");
                // Ici, tu peux déclencher la mort du joueur ou une autre action
            }
        }
        else
        {
            if (_isDetectingPlayer)
            {
                DetectionManager.Instance.PlayerLost();  // Signaler la perte de détection au gestionnaire
                _isDetectingPlayer = false;
            }
        }
    }

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
                }
                else
                {
                    _inLight = false;
                }
            }
        }
    }

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

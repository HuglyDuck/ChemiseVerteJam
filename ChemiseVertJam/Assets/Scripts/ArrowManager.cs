using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] private InputsManager _inputsManager;
    [SerializeField] private GameObject _arrowPrefab;

    private GameObject _previousArrow;
    private GameObject _nextArrow;

    [SerializeField] private float _arrowDistanceFromCenter = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float _arrowPositionFactor = 0.5f;

    private void Start()
    {
        _previousArrow = Instantiate(_arrowPrefab);
        _nextArrow = Instantiate(_arrowPrefab);

        _previousArrow.SetActive(false);
        _nextArrow.SetActive(false);
    }

    private void Update()
    {
        GameObject currentObject = _inputsManager.GetCurrentObject();
        GameObject previousObject = _inputsManager.GetPreviousObject();
        GameObject nextObject = _inputsManager.GetNextObject();

        if (currentObject != null)
        {
            if (previousObject != null)
            {
                UpdateArrow(_previousArrow, currentObject, previousObject);
            }

            if (nextObject != null)
            {
                UpdateArrow(_nextArrow, currentObject, nextObject);
            }
        }
    }

    private void UpdateArrow(GameObject arrow, GameObject fromObject, GameObject toObject)
    {
        arrow.SetActive(true);

        Vector3 startPosition = fromObject.transform.position;
        Vector3 endPosition = toObject.transform.position;
        Vector3 arrowPosition = Vector3.Lerp(startPosition, endPosition, _arrowPositionFactor);

        arrow.transform.position = arrowPosition;

        Vector3 direction = (endPosition - startPosition).normalized;
        arrow.transform.rotation = Quaternion.LookRotation(direction);
    }
}

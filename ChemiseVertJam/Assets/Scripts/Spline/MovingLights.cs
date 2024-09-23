using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingLights : MonoBehaviour
{
    [SerializeField] private Transform _lightTarget;
    [SerializeField] private Transform _player;
    [SerializeField] private FocusLight _scriptFocusLight;


    private bool _lookAtPlayer;
    private Transform target;
    private float interpolationTime;
    private Vector3 _test;
    private Vector3 _direction;
    [SerializeField] private float _rotSpeed;

    private void OnEnable()
    {
        if(_scriptFocusLight != null)
            _scriptFocusLight._eventSneeze += _scriptFocusLight__eventSneeze;
    }

    private void _scriptFocusLight__eventSneeze()
    {
        _lookAtPlayer = !_lookAtPlayer;
    }

    private void OnDisable()
    {
        if (_scriptFocusLight != null)
            _scriptFocusLight._eventSneeze -= _scriptFocusLight__eventSneeze;
    }

    private void Update()
    {
        if (!_lookAtPlayer)
        {
            LookAtTarget(_lightTarget);
        }
        else
        {
            LookAtTarget(_player);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _lookAtPlayer = !_lookAtPlayer;

        }
    }

    private void LookAtTarget(Transform target)
    {
        _test = Vector3.MoveTowards(_test, target.position - transform.position, _rotSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(_test);

       
    }

    
}

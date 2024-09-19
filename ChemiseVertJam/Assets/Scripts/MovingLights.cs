using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLights : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _player;
    private bool _lookAtPlayer;

    private void Update()
    {
        //TEMPORARY, wii be soothed after
        if (_lookAtPlayer)
        {
            LookAtTarget(_target);
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
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargets : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;

    public Transform[] Targets => _targets;
    public static PlayerTargets Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    public static DetectionManager Instance;

    [SerializeField] public float timerDeath = 2f;
    [SerializeField] public float timer = 0f;

    private int detectionCount = 0;

   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (detectionCount <= 0)
        {
            timer -= Time.deltaTime * 2f;
            timer = Mathf.Clamp(timer, 0f, timerDeath);
        }
        
    }

    public void PlayerDetected()
    {
        detectionCount++;
    }

    public void PlayerLost()
    {
        detectionCount--;
        detectionCount = Mathf.Max(0, detectionCount);
    }
}

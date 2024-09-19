using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLightDown : MonoBehaviour
{

    [SerializeField] LightDown _scriptLightDown;
    private float _timer;
    public bool _enabled = false;
    private List<GameObject> _lights = new List<GameObject>();

    private void Start()
    {
        _lights = _scriptLightDown._lights;
    }

    public void TimerReset(float Timer)
    {
        _timer = Timer;
        _enabled = true;
    }

    private void Update()
    {
        if (_enabled && _timer <= Time.time)
        {
            _enabled = false;
            foreach (var light in _lights)
            {
                light.SetActive(true);
            }
        }
    }
}

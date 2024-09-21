using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLightDown : MonoBehaviour
{

    [SerializeField] LightDown _scriptLightDown;
    private float _timer;
    private float _timerDuration;
    public bool _enabled = false;
    private List<GameObject> _lights = new List<GameObject>();
    [SerializeField] private List<GameObject> _lights2 = new List<GameObject>();

    private void Start()
    {
        _lights = _scriptLightDown._lights;

        foreach (var light in _lights)
        {
            _lights2.Add(light.transform.parent.gameObject);
        }
    }

    public void TimerReset(float Timer)
    {
        _timer = Timer + Time.time;
        _timerDuration = Timer;
        _enabled = true;
    }

    private void Update()
    {
        if(_enabled)
        {
            float timer = (_timer - Time.time) / _timerDuration;
            Debug.Log(timer);
            foreach(GameObject obj in _lights2)
            {
                if(obj.TryGetComponent(out ChronoLightDown _scriptLightDown))
                {
                    _scriptLightDown._chronoValue = timer;
                }
            }
        }

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

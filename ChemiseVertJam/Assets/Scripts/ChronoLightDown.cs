using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoLightDown : MonoBehaviour
{
    MaterialPropertyBlock _chronoMaterial;
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] GameObject _chrono;
    public float _chronoValue { private get; set; }

    private void Start()
    {
        _chronoMaterial = new MaterialPropertyBlock();
        
    }

    private void Update()
    {
        if(!_chrono.activeInHierarchy && _chronoValue > 0) _chrono.SetActive(true);
        else if (_chrono.activeInHierarchy && _chronoValue <= 0) _chrono.SetActive(false);

        _chronoMaterial.SetFloat("_Value", _chronoValue);
        _renderer.SetPropertyBlock(_chronoMaterial);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "ScriptableObjects/GlobalData", order = 1)]
public class GlobalData : ScriptableObject
{
    [SerializeField]
    private float _mainSound;

    [SerializeField]
    private float _brightness;

    [SerializeField]
    private float _gamma;

    public float MainSound
    {
        get { return _mainSound; }
        set { _mainSound = value; }
    }

    public float Brightness
    {
        get { return _brightness; }
        set { _brightness = value; }
    }

    public float Gamma
    {
        get { return _gamma; }
        set { _gamma = value; }
    }
}

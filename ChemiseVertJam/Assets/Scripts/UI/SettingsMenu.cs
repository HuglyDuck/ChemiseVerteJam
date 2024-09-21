using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GlobalData _globalData;

    [Header("Sliders")]
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Slider _gammaSlider;

    [Header("References")]
    [SerializeField] private AudioMixer _master;
    [SerializeField] private VolumeProfile _currentRenderingVolume;
    private LiftGammaGain _liftGammaGain;
    private ColorAdjustments _colorAdjustments;



    private void Awake()
    {
        _soundSlider.onValueChanged.AddListener(SetSoundLevel);
        _gammaSlider.onValueChanged.AddListener(SetGammaLevel);
        _brightnessSlider.onValueChanged.AddListener(SetBrightnessLevel);
    }

    private void Start()
    {
        _soundSlider.value = _globalData.MainSound;
        _gammaSlider.value = _globalData.Gamma;
        _brightnessSlider.value = _globalData.Brightness;

        ApplySettings();

        if (_currentRenderingVolume != null)
        {
            _currentRenderingVolume.TryGet(out _liftGammaGain);
            _currentRenderingVolume.TryGet(out _colorAdjustments);
        }
    }

    private void ApplySettings()
    {
        SetSoundLevel(_globalData.MainSound);

        SetGammaLevel(_globalData.Gamma);
        SetBrightnessLevel(_globalData.Brightness);
    }

    public void SetSoundLevel(float soundLevel)
    {
        float volume = Mathf.Lerp(-80f, 0f, soundLevel);
        _master.SetFloat("Volume", volume);//need to create parameter
        _globalData.MainSound = soundLevel;
    }

    public void SetGammaLevel(float sliderValue)
    {
        float gamma = sliderValue * 2f;

        if (_liftGammaGain != null)
        {
            _liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, gamma));
        }
        _globalData.Gamma = sliderValue;
    }

    public void SetBrightnessLevel(float sliderValue)
    {
        if (_colorAdjustments != null)
        {
            _colorAdjustments.postExposure.Override(sliderValue);
        }

        _globalData.Brightness = sliderValue;
    }
}

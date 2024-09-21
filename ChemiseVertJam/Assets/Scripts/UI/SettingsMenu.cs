using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private Slider _gammaSlider;

    [SerializeField] private AudioMixer _master;
    [SerializeField] private VolumeProfile _currentRenderingVolume;

    [SerializeField] private GlobalData _globalData;

    private LiftGammaGain _liftGammaGain;
    private ColorAdjustments _colorAdjustments;

    private void Start()
    {
        _soundSlider.value = _globalData.MainSound;
        _soundSlider.onValueChanged.AddListener(SetSoundLevel);


        _gammaSlider.value = _globalData.Gamma;
        _gammaSlider.onValueChanged.AddListener(SetGammaLevel);

        _brightnessSlider.value = _globalData.Brightness;
        _brightnessSlider.onValueChanged.AddListener(SetBrightnessLevel);

        if (_currentRenderingVolume != null)
        {
            _currentRenderingVolume.TryGet(out _liftGammaGain);
            _currentRenderingVolume.TryGet(out _colorAdjustments);
        }
    }

    public void SetSoundLevel(float soundLevel)
    {
        float volume = Mathf.Lerp(-80f, 0f, soundLevel);
        _master.SetFloat("Volume", volume);
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

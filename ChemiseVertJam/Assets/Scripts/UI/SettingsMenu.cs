using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GlobalData _globalData;

    [Header("Sliders")]
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
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
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        _gammaSlider.onValueChanged.AddListener(SetGammaLevel);
        _brightnessSlider.onValueChanged.AddListener(SetBrightnessLevel);
    }

    private void Start()
    {
        LoadSettings();

        if (_currentRenderingVolume != null)
        {
            _currentRenderingVolume.TryGet(out _liftGammaGain);
            _currentRenderingVolume.TryGet(out _colorAdjustments);
        }

        ApplySettings();
    }

    private void LoadSettings()
    {
        _soundSlider.value = _globalData.MainSound;
        _musicSlider.value = _globalData.MusicSound;
        _sfxSlider.value = _globalData.SfxSound;
        _gammaSlider.value = MapGammaToSlider(_globalData.Gamma);
        _brightnessSlider.value = MapBrightnessToSlider(_globalData.Brightness);
    }

    private void ApplySettings()
    {
        SetSoundLevel(_globalData.MainSound);
        SetMusicVolume(_globalData.MusicSound);
        SetSfxVolume(_globalData.SfxSound);
        SetGammaLevel(MapGammaToSlider(_globalData.Gamma));
        SetBrightnessLevel(MapBrightnessToSlider(_globalData.Brightness));
    }

    public void SetSoundLevel(float soundLevel)
    {
        float volume = Mathf.Lerp(-80f, 0f, soundLevel);
        _master.SetFloat("MasterVolume", volume);
        _globalData.MainSound = soundLevel;
    }

    public void SetMusicVolume(float musicLevel)
    {
        float volume = Mathf.Lerp(-80f, 0f, musicLevel);
        _master.SetFloat("MusicVolume", volume);
        _globalData.MusicSound = musicLevel;
    }

    public void SetSfxVolume(float sfxLevel)
    {
        float volume = Mathf.Lerp(-80f, 0f, sfxLevel);
        _master.SetFloat("SFXVolume", volume);
        _globalData.SfxSound = sfxLevel;
    }

    public void SetGammaLevel(float sliderValue)
    {
        float gamma = MapSliderToGamma(sliderValue);

        if (_liftGammaGain != null)
        {
            _liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, gamma));
        }
        _globalData.Gamma = gamma;
    }

    public void SetBrightnessLevel(float sliderValue)
    {
        float brightness = MapSliderToBrightness(sliderValue);

        if (_colorAdjustments != null)
        {
            _colorAdjustments.postExposure.Override(brightness);
        }

        _globalData.Brightness = brightness;
    }

    private float MapBrightnessToSlider(float brightness)
    {
        return Mathf.InverseLerp(-1f, 1f, brightness);
    }

    private float MapSliderToBrightness(float sliderValue)
    {
        return Mathf.Lerp(-1f, 1f, sliderValue);
    }

    private float MapGammaToSlider(float gamma)
    {
        return Mathf.InverseLerp(-1f, 1f, gamma);
    }

    private float MapSliderToGamma(float sliderValue)
    {
        return Mathf.Lerp(-1f, 1f, sliderValue);
    }
}

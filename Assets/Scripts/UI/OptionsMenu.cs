using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer masterMixer;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambianceSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSlider;

    [Header("Slider Text")]
    [SerializeField] private Text musicText;
    [SerializeField] private Text ambianceText;
    [SerializeField] private Text sfxText;
    [SerializeField] private Text masterText;

    [Header("Toggles")]
    [SerializeField] private Toggle muteToggle;

    private float maxMusicVol = -3;
    private float maxSFXVol;
    private float maxAmbianceVol = -12;
    private float maxMastervol = 0;

    private bool muted = false;

    private void Start()
    {
        musicSlider.value = 100;
        ambianceSlider.value = 100;
        sfxSlider.value = 100;
        masterSlider.value = 100;

        masterMixer.GetFloat("musicVol", out maxMusicVol);
        masterMixer.GetFloat("ambianceVol", out maxAmbianceVol);
        masterMixer.GetFloat("sfxVol", out maxSFXVol);

        Debug.Log(maxMusicVol);

        musicText.text = Mathf.RoundToInt(musicSlider.value).ToString();
        sfxText.text = Mathf.RoundToInt(sfxSlider.value).ToString();
        masterText.text = Mathf.RoundToInt(masterSlider.value).ToString();

        muteToggle.isOn = false;
    }

    public void SetMusicVolume(float musicVol)
    {
        float vol = (musicVol / 100) * (maxMusicVol + 80);
        vol -= 80;
        masterMixer.SetFloat("musicVol", vol);

        musicText.text = Mathf.RoundToInt(musicSlider.value).ToString();
    }

    public void SetAmbianceVolume(float ambianceVol)
    {
        float vol = (ambianceVol / 100) * (maxAmbianceVol + 80);
        vol -= 80;
        masterMixer.SetFloat("ambianceVol", vol);

        ambianceText.text = Mathf.RoundToInt(ambianceSlider.value).ToString();
    }

    public void SetSFXVol(float sfxVol)
    {
        float vol = (sfxVol / 100) * (maxSFXVol + 80);
        vol -= 80;
        masterMixer.SetFloat("sfxVol", vol);

        sfxText.text = Mathf.RoundToInt(sfxSlider.value).ToString();
    }

    public void SetMasterVol(float masterVol)
    {
        if (!muteToggle.isOn)
        {
            float vol = (masterVol / 100) * 80;
            vol -= 80;
            masterMixer.SetFloat("masterVol", vol);
        } else
        {
            Debug.Log("Set vol to 0");
            masterMixer.SetFloat("masterVol", -80);
        }

        masterText.text = Mathf.RoundToInt(masterSlider.value).ToString();
    }

    public void SetAudioMute()
    {
        if (!muteToggle.isOn)
        {
            SetMasterVol(masterSlider.value);
        } else
        {
            SetMasterVol(-80);
        }
    }
}

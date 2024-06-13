using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("ValorSliderMusica"))
        {
            float valorRecuperadoMusica = PlayerPrefs.GetFloat("ValorSliderMusica");
            _musicSlider.value = valorRecuperadoMusica;
        }

        if (PlayerPrefs.HasKey("ValorSliderSFX"))
        {
            float valorRecuperadoSFX = PlayerPrefs.GetFloat("ValorSliderSFX");
            _sfxSlider.value = valorRecuperadoSFX;
        }
    }

    /*public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }*/
    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }
}
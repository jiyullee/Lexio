using Photon.Pun;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_OptionPanel : MonoBehaviour
{
    public LevelLoader LevelLoader;
    public Toggle SoundToggle;
    public Sprite OnSound;
    public Sprite OffSound;
    Image image;

    public void Start_BackGroundMusic()
    {
        image = SoundToggle.GetComponent<Image>();
        if (PlayerPrefs.HasKey("Audio"))
        {
            int turnMusic = PlayerPrefs.GetInt("Audio");
            if (turnMusic == 0)
            {
                image.sprite = OffSound;
                SoundToggle.isOn = false;
                return;
            }
        }
        SoundManager.Instance.TurnOn_LobbyBackSound();
    }
    public void OnClick_TurnOnOffMusic()
    {
        if (!SoundToggle.isOn)
        {
            PlayerPrefs.SetInt("Audio", 0);
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.mute = true;
            }
            SoundManager.Instance.TurnOff_LobbyBackSound();
            image.sprite = OffSound;
        }
        else
        {
            PlayerPrefs.SetInt("Audio", 1);
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.mute = false;
            }
            
            SoundManager.Instance.TurnOn_LobbyBackSound();
            image.sprite = OnSound;
        }
    }

    public void OnClick_DisappearPanel()
    {
        Lobby_MainCanvasManager.Instance.buttonSound.Play();
        gameObject.SetActive(false);
    }
    public void OnClick_LogOut()
    {
        Lobby_MainCanvasManager.Instance.buttonSound.Play();
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.SetInt("AutoLogin", 0);
        LevelLoader.LoadNextLevel("SignIn");

    }
}

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
    private void Start()
    {
        image = SoundToggle.GetComponent<Image>();
    }
    public void OnClick_TurnOnOffMusic()
    {
        if (!SoundToggle.isOn)
        {
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

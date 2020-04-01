using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_GameOption : MonoBehaviour
{
    public Toggle SoundToggle;
    public Sprite OnSound;
    public Sprite OffSound;
    Image image;

    public void Start_BackGroundMusic ()
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
        SoundManager.Instance.SelectRoomSound();
    }
    public void OnClick_TurnOnOffMusic()
    {
        if (!SoundToggle.isOn)
        {
            PlayerPrefs.SetInt("Audio", 0);
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach(AudioSource audioSource in audioSources)
            {
                audioSource.mute = true;
            }
            SoundManager.Instance.TurnOff_RoomBackSound();
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
            if (SoundManager.Instance.RoomBackSound.clip == null)
                SoundManager.Instance.SelectRoomSound();
            SoundManager.Instance.TurnOn_RoomBackSound();
            image.sprite = OnSound;
        }
    }

    public void OnClick_DisappearPanel()
    {
        Room_MainCanvasManager.Instance.buttonSound.Play();
        gameObject.SetActive(false);
    }
}

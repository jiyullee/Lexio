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
    private void Start()
    {
        image = SoundToggle.GetComponent<Image>();
    }
    public void OnClick_TurnOnOffMusic()
    {
        if (!SoundToggle.isOn)
        {
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
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.mute = false;
            }
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

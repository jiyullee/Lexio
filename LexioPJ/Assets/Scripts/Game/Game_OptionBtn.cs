using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_OptionBtn : MonoBehaviour
{
    public static Game_OptionBtn Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Game_OptionBtn>();

            return instance;
        }
    }
    private static Game_OptionBtn instance;
    public Toggle SoundToggle;
    public Sprite OnSound;
    public Sprite OffSound;
    Image image;

    public void Awake()
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
        SoundManager.Instance.TurnOn_GameBackSound();
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
            SoundManager.Instance.TurnOff_GameBackSound();
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
            SoundManager.Instance.TurnOn_GameBackSound();
            image.sprite = OnSound;
        }
    }

    public void OnClick_DisappearPanel()
    {
        gameObject.SetActive(false);
    }
}

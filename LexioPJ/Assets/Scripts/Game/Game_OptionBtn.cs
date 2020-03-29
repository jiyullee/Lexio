using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_OptionBtn : MonoBehaviour
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
            SoundManager.Instance.TurnOff_GameBackSound();
            image.sprite = OffSound;
        }
        else
        {
            SoundManager.Instance.TurnOn_GameBackSound();
            image.sprite = OnSound;
        }
    }

    public void OnClick_DisappearPanel()
    {
        gameObject.SetActive(false);
    }
}

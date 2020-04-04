using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPass : MonoBehaviour
{

    public float lifeTime;
    AudioSource AllPassSound;
    private void Awake()
    {
        AllPassSound = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        if (Game_OptionBtn.Instance.SoundToggle.isOn)
            AllPassSound.mute = false;
        else
            AllPassSound.mute = true;
        AllPassSound.Play();
        transform.SetAsLastSibling();
        PlayerScript[] playerScripts = GameObject.FindObjectsOfType<PlayerScript>();
        for (int i = 0; i < playerScripts.Length; i++)
        {
            playerScripts[i].turnStateObj.SetActive(false);
        }
        StartCoroutine(DisableSelf());
    }
    IEnumerator DisableSelf()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}

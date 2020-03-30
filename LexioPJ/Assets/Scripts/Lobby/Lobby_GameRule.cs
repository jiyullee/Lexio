using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_GameRule : MonoBehaviour
{
    int index = 0;
    public Text nowIndex;
    public GameObject[] panels;
    private void Start()
    {
        index = 0;
        for (int i = 1; i < panels.Length; i++)
            panels[i].SetActive(false);
    }

    private void OnDisable()
    {
        index = 0;
        for (int i = 1; i < panels.Length; i++)
            panels[i].SetActive(false);
    }
    public void OnClick_NextPanel()
    {
        Lobby_MainCanvasManager.Instance.buttonSound.Play();
        index++;
        if (index >= panels.Length)
            index = panels.Length - 1;
        nowIndex.text = string.Format("{0} / {1}", index + 1, panels.Length);
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(false);
        panels[index].SetActive(true);
    }
    public void OnClick_PreviousPanel()
    {
        Lobby_MainCanvasManager.Instance.buttonSound.Play();
        index--;
        if (index <= 0)
            index = 0;
        nowIndex.text = string.Format("{0} / {1}", index + 1, panels.Length);
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(false);
        panels[index].SetActive(true);
    }
    public void OnClick_DisappearPanel()
    {
        Lobby_MainCanvasManager.Instance.buttonSound.Play();
        gameObject.SetActive(false);
    }
}

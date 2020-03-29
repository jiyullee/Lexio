using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SignIn_InputName : MonoBehaviour
{
    public static SignIn_InputName Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SignIn_InputName>();

            return instance;
        }
    }
    private static SignIn_InputName instance;

    private string playerName;
    public InputField inputName;


    private void SaveName()
    {
        PlayerPrefs.SetString("Name", playerName);
        PhotonNetwork.LoadLevel("Lobby");
    }

    private void LoadName()
    {
        if (PlayerPrefs.HasKey("Name"))
        {
            playerName = PlayerPrefs.GetString("Name");
        }
        
    }

    public void OnClick_InputName()
    {
        if (2 <= inputName.text.Length && inputName.text.Length <= 6)
        {
            playerName = inputName.text;
            SaveName();
        }
    }

    public string GetName()
    {
        return playerName;
    }
}

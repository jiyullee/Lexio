using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public PlayerNetwork PlayerNetwork;
    public void Onclick_RoomQuit()
    {
        PlayerNetwork.QuitGame();
    }

    public void OnClick_CancelQuit()
    {
        gameObject.SetActive(false);
    }
    public void Onclick_Quit()
    {
        Application.Quit();
    }
}

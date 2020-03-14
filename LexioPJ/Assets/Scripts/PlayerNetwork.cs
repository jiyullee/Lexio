using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviourPun
{
    public void LeftRoom()
    {
        Room_Player[] players = GameObject.FindObjectsOfType<Room_Player>();
        foreach (Room_Player player in players)
        {
            player.LeftRoom(PhotonNetwork.LocalPlayer);
        }
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
}

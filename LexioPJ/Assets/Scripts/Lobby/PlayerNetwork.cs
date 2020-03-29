using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviourPunCallbacks
{
    public void LeftRoom()
    {
        photonView.RPC("RPC_DestroyPlayer", RpcTarget.All, PhotonNetwork.NickName);
        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Remove("PlayerCount");
        room.CustomProperties.Add("PlayerCount", (PhotonNetwork.CurrentRoom.PlayerCount) - 1);
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
        if (PhotonNetwork.IsMasterClient)
        {
            if (!(PhotonNetwork.PlayerList.Length == 1))
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
                room.CustomProperties.Remove("MasterName");
                room.CustomProperties.Add("MasterName", PhotonNetwork.PlayerListOthers[0].NickName);
                PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
                RoomManager.Instance.SwitchMaster();
            }

        }               
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    private void RPC_DestroyPlayer(string name)
    {
        Room_Player[] room_Players = FindObjectsOfType<Room_Player>();
        for (int i = 0; i < room_Players.Length; i++)
        {
            if (room_Players[i].photonView.Owner.NickName == name)
                Destroy(room_Players[i].gameObject);
        }
    }
    public void ReturntoRoom()
    {
        Room_Player[] players = GameObject.FindObjectsOfType<Room_Player>();
        foreach (Room_Player player in players)
        {
            player.LeftRoom(PhotonNetwork.LocalPlayer);
        }
        PhotonNetwork.LeaveRoom();
    }

    public void QuitGame()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        int playerCount = (int)room.CustomProperties["PlayerCount"];
        room.CustomProperties.Remove("PlayerCount");
        room.CustomProperties.Add("PlayerCount", room.PlayerCount);
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);

        if (PhotonNetwork.IsMasterClient)
        {
            if (!(PhotonNetwork.PlayerList.Length == 1))
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);              
                room.CustomProperties.Remove("MasterName");
                room.CustomProperties.Add("MasterName", PhotonNetwork.PlayerListOthers[0].NickName);
                PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
                RoomManager.Instance.SwitchMaster();
            }

        }
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        Application.Quit();
    }

    public override void OnLeftRoom()
    {       
        SceneManager.LoadScene("Lobby");
    }
 
}

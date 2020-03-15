using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerListObj;
    public GameObject PlayerPrefab;
    public Button startBtn;
    public Button RoomOptBtn;
    static int spawnIndex = 0;
    public Room_ChangeRoomInfo Room_ChangeRoomInfo;
    public Room_ChatService Room_ChatService;
    void Start()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Remove("State");
        room.CustomProperties.Add("State", "대기");

        if (!PhotonNetwork.IsMasterClient)
        {
            SpawnOtherPlayers();
            startBtn.gameObject.SetActive(false);
            RoomOptBtn.gameObject.SetActive(false);

        }
        else
        {
            startBtn.gameObject.SetActive(true);
            RoomOptBtn.gameObject.SetActive(true);
        }
        SpawnPlayer();
        Room_ChangeRoomInfo.ChangeRoomNameText();
        Random r = new Random(Guid.NewGuid().GetHashCode());
        ExitGames.Client.Photon.Hashtable CustomRoomProperty = new ExitGames.Client.Photon.Hashtable();
        CustomRoomProperty.Clear();
        CustomRoomProperty.Add("ImageRand", r.Next(0, 8));
        PhotonNetwork.SetPlayerCustomProperties(CustomRoomProperty);

    }

    private void SpawnOtherPlayers()
    {
        for(int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            for (int j = 0; j < playerListObj.Length; j++)
            {
                if (playerListObj[j].transform.childCount == 0)
                {
                    spawnIndex = j;
                    break;
                }
            }
            var spawnPosition = playerListObj[spawnIndex % playerListObj.Length];

            GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
            obj.transform.SetParent(spawnPosition.transform);
            obj.GetComponent<Room_Player>().SetNameText(PhotonNetwork.PlayerListOthers[i].NickName);
            obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerListOthers[i]);
        }
    }

    private void SpawnPlayer()
    {
        for(int i = 0; i < playerListObj.Length; i++)
        {
            if (playerListObj[i].transform.childCount == 0)
            {
                spawnIndex = i;
                break;
            }
        }
        var spawnPosition = playerListObj[spawnIndex % playerListObj.Length];
        
        GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
        obj.transform.SetParent(spawnPosition.transform);
        obj.GetComponent<Room_Player>().SetNameText(PlayerPrefs.GetString("Name"));
        obj.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        for (int i = 0; i < playerListObj.Length; i++)
        {
            if (playerListObj[i].transform.childCount == 0)
            {
                spawnIndex = i;
                break;
            }
        }
        var spawnPosition = playerListObj[spawnIndex % playerListObj.Length];

        GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
        obj.transform.SetParent(spawnPosition.transform);
        obj.GetComponent<Room_Player>().SetNameText(newPlayer.NickName);
        obj.GetPhotonView().TransferOwnership(newPlayer);
    }

    public void OnClick_StartGame()
    {
        photonView.RPC("RPC_DisConnectChatChannel", RpcTarget.All);

        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Remove("State");
        room.CustomProperties.Add("State", "게임 중");
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
        int playerCount = PhotonNetwork.PlayerList.Length;

        if(playerCount <= 2)
        {
            PhotonNetwork.LoadLevel("Game_2Players");
        }else if(playerCount == 3)
        {
            PhotonNetwork.LoadLevel("Game_3Players");
        }
        else if (playerCount == 4)
        {
            PhotonNetwork.LoadLevel("Game_4Players");
        }
        else if (playerCount == 5)
        {
            PhotonNetwork.LoadLevel("Game_5Players");
        }
        else if (playerCount == 6)
        {
            PhotonNetwork.LoadLevel("Game_6Players");
        }

    }
    [PunRPC]
    private void RPC_DisConnectChatChannel()
    {
        Room_ChatService.DisConnectChannel();
    }
}

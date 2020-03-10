using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerListObj;
    public GameObject PlayerPrefab;
    public Button startBtn;
    public Button RoomOptBtn;
    int statrIndex = 0;
    int newStartIndex = 1;
    void Start()
    {
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber;

        if (!PhotonNetwork.IsMasterClient)
        {
            int playerIndex = 0;
            for (int i = localPlayerIndex - 1; i > 0; i--)
            {
                var spawnPosition = playerListObj[statrIndex++ % playerListObj.Length];

                GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
                obj.transform.SetParent(spawnPosition.transform);
                obj.GetComponent<Room_Player>().SetNameText(PhotonNetwork.PlayerListOthers[playerIndex].NickName);
                obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerListOthers[playerIndex]);
                playerIndex++;
            }
            startBtn.gameObject.SetActive(false);
            RoomOptBtn.gameObject.SetActive(false);

        }
        else
        {
            startBtn.gameObject.SetActive(true);
            RoomOptBtn.gameObject.SetActive(true);
        }
        SpawnPlayer();

        
    }

    void SpawnPlayer()
    {
        var spawnPosition = playerListObj[statrIndex % playerListObj.Length];

        GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
        obj.transform.SetParent(spawnPosition.transform);
        obj.GetComponent<Room_Player>().SetNameText(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var spawnPosition = playerListObj[newPlayer.ActorNumber - 1 % playerListObj.Length];
        GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
        obj.transform.SetParent(spawnPosition.transform);
        obj.GetComponent<Room_Player>().SetNameText(newPlayer.NickName);
        obj.GetPhotonView().TransferOwnership(newPlayer);
    }

    public void OnClick_StartGame()
    {
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
}

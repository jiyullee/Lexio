using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_4Players : MonoBehaviour
{
    public Canvas canvas;
    public GameObject playerPrefabs;
    public Transform[] spawnPositions;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[i - 1];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.GetComponent<PlayerScript>().OnRegisterPanel(PhotonNetwork.PlayerList.Length, i);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<Player_UserInfo>().AccessInfo(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
                obj.GetComponent<PlayerScript>().SetColor();
                obj.GetComponent<PlayerScript>().owner = player;
            }
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1])
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1 || i == 2)
                {
                    player = PhotonNetwork.PlayerListOthers[i];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.GetComponent<PlayerScript>().OnRegisterPanel(PhotonNetwork.PlayerList.Length, i);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<Player_UserInfo>().AccessInfo(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
                obj.GetComponent<PlayerScript>().SetColor();
                obj.GetComponent<PlayerScript>().owner = player;
            }
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2])
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1)
                {
                    player = PhotonNetwork.PlayerListOthers[2];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if(i == 2)
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[1];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.GetComponent<PlayerScript>().OnRegisterPanel(PhotonNetwork.PlayerList.Length, i);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<Player_UserInfo>().AccessInfo(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
                obj.GetComponent<PlayerScript>().SetColor();
                obj.GetComponent<PlayerScript>().owner = player;
            }
        }
        else
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1)
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 2)
                {
                    player = PhotonNetwork.PlayerListOthers[1];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[2];
                    obj = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.GetComponent<PlayerScript>().OnRegisterPanel(PhotonNetwork.PlayerList.Length, i);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<Player_UserInfo>().AccessInfo(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
                obj.GetComponent<PlayerScript>().SetColor();
                obj.GetComponent<PlayerScript>().owner = player;
            }
        }
    }
}

﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_3Players : MonoBehaviourPun
{
    public Canvas canvas;
    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;

    private void Start()
    {
        SpawnPlayer();
    }
   
    void SpawnPlayer()
    {
        if(PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefabs[0].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1)
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefabs[1].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[1];
                    obj = PhotonNetwork.Instantiate(playerPrefabs[2].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
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
                    obj = PhotonNetwork.Instantiate(playerPrefabs[0].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1)
                {
                    player = PhotonNetwork.PlayerListOthers[1];
                    obj = PhotonNetwork.Instantiate(playerPrefabs[2].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefabs[1].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
            }
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2])
        {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefabs[0].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1)
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefabs[1].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[1];
                    obj = PhotonNetwork.Instantiate(playerPrefabs[2].name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
            }
        }
    }
}
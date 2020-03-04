﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_6Players : MonoBehaviour
{
    public Canvas canvas;
    public GameObject playerPrefab;
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
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[i - 1];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
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
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1 || i == 2 || i == 3 || i == 4)
                {
                    player = PhotonNetwork.PlayerListOthers[i];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
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
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1 || i == 2 || i == 3)
                {
                    player = PhotonNetwork.PlayerListOthers[i + 1];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 4)
                {
                    player = PhotonNetwork.PlayerListOthers[0];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[1];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
            }
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[3])
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1 || i == 2)
                {
                    player = PhotonNetwork.PlayerListOthers[i + 2];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[i - 3];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
            }
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[4])
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else if (i == 1)
                {
                    player = PhotonNetwork.PlayerListOthers[4];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[i - 2];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
            }
        }
        else if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[5])
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player;
                GameObject obj;
                if (i == 0)
                {
                    player = PhotonNetwork.LocalPlayer;
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                else
                {
                    player = PhotonNetwork.PlayerListOthers[i - 1];
                    obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[i].position, spawnPositions[i].rotation);
                }
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(player.NickName);
                obj.GetPhotonView().TransferOwnership(player);
            }
        }

    }
}

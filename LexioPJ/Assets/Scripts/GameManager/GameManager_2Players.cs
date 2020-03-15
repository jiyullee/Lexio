using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class GameManager_2Players : MonoBehaviourPun
{
    public Canvas canvas;
    public GameObject[] playerPrefab;
    int[] rands;
    public Transform[] spawnPositions;
    private void Awake()
    {

        
        
    }
    private void Start()
    {        
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var spawnPosition = spawnPositions[i % spawnPositions.Length];                     
            if(i == 0)
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab[0].name, spawnPosition.position, spawnPosition.rotation);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(PhotonNetwork.LocalPlayer.NickName);
                obj.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
                obj.GetComponent<PlayerScript>().SetColor();
            }
            else
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab[1].name, spawnPosition.position, spawnPosition.rotation);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(PhotonNetwork.PlayerListOthers[0].NickName);               
                obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerListOthers[0]);
                obj.GetComponent<PlayerScript>().SetColor();
            }
               
        }

        
    }

    public void GameOver()
    {

    }
}

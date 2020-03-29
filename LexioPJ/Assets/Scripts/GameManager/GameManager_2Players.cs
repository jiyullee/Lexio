using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class GameManager_2Players : MonoBehaviourPunCallbacks
{
    public Canvas canvas;
    public GameObject playerPrefab;
    int[] rands;
    public Transform[] spawnPositions;
    string roomName;
    private void Awake()
    {

        
        
    }
    private void Start()
    {
        roomName = PhotonNetwork.CurrentRoom.Name;
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var spawnPosition = spawnPositions[i % spawnPositions.Length];                     
            if(i == 0)
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
                obj.GetComponent<PlayerScript>().OnRegisterPanel(PhotonNetwork.CurrentRoom.PlayerCount, i);
                obj.transform.SetParent(canvas.transform);
                obj.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
                obj.GetComponent<PlayerScript>().SetColor();
                obj.GetComponent<PlayerScript>().owner = PhotonNetwork.LocalPlayer;
                obj.GetComponent<Player_UserInfo>().AccessInfo(PhotonNetwork.LocalPlayer.NickName);
            }
            else
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
                obj.GetComponent<PlayerScript>().OnRegisterPanel(PhotonNetwork.CurrentRoom.PlayerCount, i);
                obj.transform.SetParent(canvas.transform);              
                obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerListOthers[0]);
                obj.GetComponent<PlayerScript>().SetColor();
                obj.GetComponent<PlayerScript>().owner = PhotonNetwork.PlayerListOthers[0];
                obj.GetComponent<Player_UserInfo>().AccessInfo(PhotonNetwork.PlayerListOthers[0].NickName);
            }
               
        }

        
    }

}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_2Players : MonoBehaviourPun
{
    public Canvas canvas;
    public GameObject[] playerPrefab;

    public Transform[] spawnPositions;
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
            }
            else
            {
                GameObject obj = PhotonNetwork.Instantiate(playerPrefab[1].name, spawnPosition.position, spawnPosition.rotation);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(PhotonNetwork.PlayerListOthers[0].NickName);
                obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerListOthers[0]);
            }
               
        }
    }

    public void GameOver()
    {

    }
}

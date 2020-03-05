using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }
    private static GameManager instance;
    CardManager cardManager;
    public Canvas canvas;
    public Transform[] spawnPositions;
    public GameObject playerPrefab;
    int index = 1;
    int statrIndex = 5;
    int newStartIndex = 1;
    public List<GameObject> playerObjects = new List<GameObject>();
    int turnIndex = 0;
    bool isGameStart;

    void Start()
    {
        cardManager = GetComponent<CardManager>();
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        statrIndex = 7 - localPlayerIndex;

        if (!PhotonNetwork.IsMasterClient)
        {
            for (int i = localPlayerIndex - 1; i > 0; i--)
            {
                int playerIndex = 0;
                var spawnPosition = spawnPositions[statrIndex % spawnPositions.Length];

                GameObject obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<PlayerScript>().SetPlayerName(PhotonNetwork.PlayerListOthers[playerIndex].NickName);
                obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerListOthers[playerIndex]);
                playerIndex++;
            }
        }
        SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            Game_MainCanvasManager.Instance.GameStartButton.gameObject.SetActive(true);
        }
    }

    void SpawnPlayer()
    {
        var spawnPosition = spawnPositions[0 % spawnPositions.Length];

        GameObject obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
        obj.transform.SetParent(canvas.transform);
        obj.GetComponent<PlayerScript>().SetPlayerName(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var spawnPosition = spawnPositions[newStartIndex++ % spawnPositions.Length];
        GameObject obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
        obj.transform.SetParent(canvas.transform);
        obj.GetComponent<PlayerScript>().SetPlayerName(newPlayer.NickName);
        obj.GetPhotonView().TransferOwnership(newPlayer);
    }

   
}


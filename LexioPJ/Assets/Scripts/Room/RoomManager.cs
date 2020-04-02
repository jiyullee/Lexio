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
    public static RoomManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<RoomManager>();

            return instance;
        }
    }

    private static RoomManager instance;
    public Room_InfoPanel Room_UserInfo;
    public GameObject[] playerListObj;
    public GameObject PlayerPrefab;
    public Button startBtn;
    public Button RoomOptBtn;
    static int spawnIndex = 0;
    public Room_ChangeRoomInfo Room_ChangeRoomInfo;
    public Room_ChatService Room_ChatService;
    public string[] PlayfabIDs;
    public Image RoomLock;
    public LevelLoader LevelLoader;

    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }
    public void Start()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Remove("PlayerCount");
        room.CustomProperties.Add("PlayerCount", (int)PhotonNetwork.CurrentRoom.PlayerCount);    
        room.CustomProperties.Remove("State");
        room.CustomProperties.Add("State", "대기");
        PhotonNetwork.CurrentRoom.IsVisible = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
        if (room.CustomProperties.ContainsKey("Restart"))
        {
            if ((bool)room.CustomProperties["Restart"])
            {
               // SpawnOtherPlayers();
                Restart_SpawnPlayers();
            }
            else
            {
                SpawnOtherPlayers();
            }
        }
        else
        {
            SpawnOtherPlayers();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            room.CustomProperties.Remove("MasterName");
            room.CustomProperties.Add("MasterName", PhotonNetwork.MasterClient.NickName);
            PhotonNetwork.SetPlayerCustomProperties(room.CustomProperties);
            startBtn.gameObject.SetActive(true);
            RoomOptBtn.gameObject.SetActive(true);
        }
        else
        {
            startBtn.gameObject.SetActive(false);
            RoomOptBtn.gameObject.SetActive(false);
        }

        Room_ChangeRoomInfo.ChangeRoomName();
        Random r = new Random(Guid.NewGuid().GetHashCode());
        if (room.CustomProperties.ContainsKey("ImageRand"))
            room.CustomProperties.Remove("ImageRand");
        room.CustomProperties.Add("ImageRand", r.Next(0, 8));
        PhotonNetwork.SetPlayerCustomProperties(room.CustomProperties);

    }
    public void SwitchMaster()
    {
        photonView.RPC("RPC_SwitchMaster", RpcTarget.All);
        
    }

    [PunRPC]
    private void RPC_SwitchMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.gameObject.SetActive(true);
            RoomOptBtn.gameObject.SetActive(true);
        }
        else
        {
            startBtn.gameObject.SetActive(false);
            RoomOptBtn.gameObject.SetActive(false);
        }
        Room_Player[] room_Players = FindObjectsOfType<Room_Player>();
        for (int i = 0; i < room_Players.Length; i++)
        {
            if(room_Players[i].photonView.Owner.IsMasterClient)
                room_Players[i].OnMasterSign();
        }
    }
    private void Restart_SpawnPlayers()
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var spawnPosition = playerListObj[i % playerListObj.Length];
            GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
            obj.transform.SetParent(spawnPosition.transform);
            obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[i]);
            obj.GetComponent<Room_Player>().OnMasterSign();      
            obj.GetComponent<Player_UserInfo>().AccessInfo(PhotonNetwork.PlayerList[i].NickName);
            
        }
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
            obj.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerListOthers[i]);
            obj.GetComponent<Room_Player>().OnMasterSign();
            obj.GetComponent<Player_UserInfo>().AccessInfo(PhotonNetwork.PlayerListOthers[i].NickName);
            
        }
        SpawnPlayer();
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
        obj.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
        obj.GetComponent<Room_Player>().OnMasterSign();
        obj.GetComponent<Player_UserInfo>().AccessInfo(PhotonNetwork.LocalPlayer.NickName);
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
        obj.GetPhotonView().TransferOwnership(newPlayer);
        obj.GetComponent<Room_Player>().OnMasterSign();
        obj.GetComponent<Player_UserInfo>().AccessInfo(newPlayer.NickName);
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine(EnterPlayer());
    }
    IEnumerator EnterPlayer()
    {
        startBtn.interactable = false;

        yield return new WaitForSeconds(3.0f);

        startBtn.interactable = true;
    }
    public void OnClick_StartGame()
    {
        int playerCount = PhotonNetwork.PlayerList.Length;
        if (playerCount == 1)
            return;

        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Remove("State");
        room.CustomProperties.Add("State", "게임 중");
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
        
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.DestroyAll();
        if (playerCount == 2)
        {
            photonView.RPC("RPC_DisConnectChatChannel", RpcTarget.All);
            LevelLoader.LoadNextLevel("Game_2Players");
        }else if(playerCount == 3)
        {
            photonView.RPC("RPC_DisConnectChatChannel", RpcTarget.All);
            LevelLoader.LoadNextLevel("Game_3Players");
        }
        else if (playerCount == 4)
        {
            photonView.RPC("RPC_DisConnectChatChannel", RpcTarget.All);
            LevelLoader.LoadNextLevel("Game_4Players");
        }
        else if (playerCount == 5)
        {
            photonView.RPC("RPC_DisConnectChatChannel", RpcTarget.All);
            LevelLoader.LoadNextLevel("Game_5Players");
        }
        else if (playerCount == 6)
        {
            photonView.RPC("RPC_DisConnectChatChannel", RpcTarget.All);
            LevelLoader.LoadNextLevel("Game_6Players");
        }

    }

    [PunRPC]
    private void RPC_DisConnectChatChannel()
    {
        Room_ChatService.DisConnectChannel();
    }
}

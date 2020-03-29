using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_ChangeRoomInfo : MonoBehaviourPunCallbacks
{
    public InputField passWord;
    public Dropdown playerCount;
    public Dropdown betting;
    public Text RoomNameText;
    public Toggle passwordToggle;
    public Image LockImage;
    public Sprite Lock;
    public Sprite UnLock;
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        ExitGames.Client.Photon.Hashtable hashtable = room.CustomProperties;
        passWord.text = (string)hashtable["Password"];
        if(passWord.text == "")
        {
            LockImage.sprite = UnLock;
            passwordToggle.isOn = false;
            passWord.interactable = false;
        }
        else
        {
            LockImage.sprite = Lock;
            passwordToggle.isOn = true;
            passWord.interactable = true;
        }
            
    }

    public void ChangeRoomName()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        ExitGames.Client.Photon.Hashtable hashtable = room.CustomProperties;
        if ((string)hashtable["Password"] == "")
        {
            LockImage.sprite = UnLock;
            RoomNameText.text = string.Format("{0} [{1}인] [판돈 {2}]", room.Name, room.MaxPlayers, (string)hashtable["Betting"]);
        }
        else
        {
            LockImage.sprite = Lock;
            RoomNameText.text = string.Format("{0} [{1}인] [판돈 {2}] [암호 : {3}]", room.Name, room.MaxPlayers, (string)hashtable["Betting"], (string)hashtable["Password"]);
        }
        Room_MainCanvasManager.Instance.ChangeRoomName(RoomNameText.text);
        gameObject.SetActive(false);
    }

    [PunRPC]
    private void RPC_ChangeRoomNameText(string roomName)
    {
        RoomNameText.text = roomName;       
    }

    public void TogglePassWord()
    {
        if (passWord.IsInteractable())
        {
            passWord.interactable = false;
            passWord.text = "";
        }
        else
            passWord.interactable = true;
    }

    public void OnClick_ChangeRoomInfo()
    {
        string bettingAmount = "";
        if (betting.value == 0)
            bettingAmount = "백원";
        else if (betting.value == 1)
            bettingAmount = "천원";
        else if (betting.value == 2)
            bettingAmount = "만원";
        else if (betting.value == 3)
            bettingAmount = "십만원";
        else if (betting.value == 4)
            bettingAmount = "백만원";
        
        PhotonNetwork.CurrentRoom.MaxPlayers = (byte)(playerCount.value + 2);
         
        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Clear();
        room.CustomProperties.Add("RoomName", room.Name);
        room.CustomProperties.Add("Password", passWord.text);
        room.CustomProperties.Add("MasterName", PhotonNetwork.PlayerList[(room.masterClientId - 1) % PhotonNetwork.PlayerList.Length].NickName);
        room.CustomProperties.Add("Betting", bettingAmount);
        room.CustomProperties.Add("PlayerCount", (int)PhotonNetwork.CurrentRoom.PlayerCount);
        room.CustomProperties.Add("MaxPlayer", (int)room.MaxPlayers);
        room.CustomProperties.Add("State", "대기");
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
        ChangeRoomName();
    }

    public void OnClick_DisappearPanel()
    {
        playerCount.navigation = Navigation.defaultNavigation;
        betting.navigation = Navigation.defaultNavigation;

        
        gameObject.SetActive(false);
        

    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Remove("PlayerCount");
        room.CustomProperties.Remove("MaxPlayer");

        room.CustomProperties.Add("PlayerCount", (int)PhotonNetwork.CurrentRoom.PlayerCount);
        room.CustomProperties.Add("MaxPlayer", (int)room.MaxPlayers);
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        room.CustomProperties.Remove("PlayerCount");
        room.CustomProperties.Remove("MaxPlayer");

        room.CustomProperties.Add("PlayerCount", (int)PhotonNetwork.CurrentRoom.PlayerCount);
        room.CustomProperties.Add("MaxPlayer", (int)room.MaxPlayers);
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
    }

}

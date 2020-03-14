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

    private void OnEnable()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        ExitGames.Client.Photon.Hashtable hashtable = room.CustomProperties;
        passWord.text = (string)hashtable["Password"];
        if(passWord.text == "")
        {
            passwordToggle.isOn = false;
            passWord.interactable = false;
        }
        else
        {
            passwordToggle.isOn = true;
            passWord.interactable = true;
        }
            
    }

    public void ChangeRoomNameText()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        ExitGames.Client.Photon.Hashtable hashtable = room.CustomProperties;
        if ((string)hashtable["Password"] == "")
        {
            RoomNameText.text = string.Format("{0} [{1}인] [타일 당 {2}]", room.Name, room.MaxPlayers, (string)hashtable["Betting"]);
        }
        else
        {
            RoomNameText.text = string.Format("{0} [{1}인] [타일 당 {2}] [암호 : {3}]", room.Name, room.MaxPlayers, (string)hashtable["Betting"], (string)hashtable["Password"]);
        }

        gameObject.SetActive(false);
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
            bettingAmount = "1백원";
        else if (betting.value == 1)
            bettingAmount = "1천원";
        else if (betting.value == 2)
            bettingAmount = "1만원";
        else if (betting.value == 3)
            bettingAmount = "10만원";
        else if (betting.value == 4)
            bettingAmount = "100만원";
        
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
        ChangeRoomNameText();
        
        
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

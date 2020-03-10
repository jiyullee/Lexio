using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_ChangeRoomInfo : MonoBehaviour
{
    public InputField roomName;
    public InputField passWord;
    public Dropdown playerCount;
    public Dropdown betting;
    public Text RoomNameText;

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

        RoomInfo room = PhotonNetwork.CurrentRoom;
        ExitGames.Client.Photon.Hashtable hashtable = room.CustomProperties;
        hashtable.Add("RoomName", roomName.text);
        hashtable.Add("Password", passWord.text);
        hashtable.Add("Betting", bettingAmount);
        PhotonNetwork.CurrentRoom.MaxPlayers = (byte)(playerCount.value + 2);

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

        ChangeRoomNameText();
    }
}

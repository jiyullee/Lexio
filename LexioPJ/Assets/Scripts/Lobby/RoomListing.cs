using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class RoomListing : MonoBehaviour
{
    public Text RoomNoText;
    public Text RoomNameText;
    public Text PlayerCountText;
    public Text MasterNameText;
    public Text bettingAmountText;
    public Text stateText;
    bool isClose;
    public string RoomName { get; private set; }
    public int MaxPlayerCount { get; private set; }
    public int CurrentPlayerCount { get; private set; }
    public bool Updated { get; set; }

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => LobbyManager.Instance.OnClickJoinRoom(RoomNameText.text, isClose));
    }

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void ChangeRoomInfo(string roomName, string password, string masterName, int playerCount,int maxPlayer, string betting)
    {
        if (password != "")
            isClose = true;
        RoomNameText.text = roomName;
        MasterNameText.text = masterName;
        PlayerCountText.text = string.Format("{0}/{1}", playerCount, maxPlayer);
        bettingAmountText.text = betting.ToString();

    }
}

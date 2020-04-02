using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomListing : MonoBehaviourPunCallbacks
{
    public Text RoomNameText;
    public Text PlayerCountText;
    public Text MasterNameText;
    public Text BettingAmountText;
    public Text StateText;
    public GameObject Lock;

    public Sprite LockSprite;
    public Sprite UnLockSprite;
    bool isClose;

    Image LockImage;
    public RoomInfo room;
    public ExitGames.Client.Photon.Hashtable CustomRoomProperty;
    public string RoomName { get; private set; }
    public int MaxPlayerCount { get; private set; }
    public int CurrentPlayerCount { get; private set; }
    public bool Updated { get; set; }
    Button button;
    void Start()
    {
        LockImage = Lock.GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => LobbyManager.Instance.OnClickJoinRoom(RoomNameText.text, isClose));
    }

    private void Update()
    { 
        
        if (CustomRoomProperty == null)
            return;
        if(CustomRoomProperty.ContainsKey("RoomName"))
            RoomNameText.text = (string)CustomRoomProperty["RoomName"];
        if (CustomRoomProperty.ContainsKey("Password"))
        {
            if((string)CustomRoomProperty["Password"] == "")
            {
                isClose = false;
                LockImage.sprite = UnLockSprite;
            }
            else
            {
                isClose = true;
                LockImage.sprite = LockSprite;
            }
        }
        
        if(CustomRoomProperty.ContainsKey("PlayerCount") && CustomRoomProperty.ContainsKey("MaxPlayer"))
        {
            PlayerCountText.text = ((int)CustomRoomProperty["PlayerCount"]).ToString() + "/" +  ((int)CustomRoomProperty["MaxPlayer"]).ToString();
        }
        if (CustomRoomProperty.ContainsKey("MasterName"))
        {
            MasterNameText.text = (string)CustomRoomProperty["MasterName"];
        }
        if (CustomRoomProperty.ContainsKey("Betting"))
        {
            BettingAmountText.text = (string)CustomRoomProperty["Betting"];
        }
        if (CustomRoomProperty.ContainsKey("State"))
        {
            StateText.text = (string)CustomRoomProperty["State"];
            if((string)CustomRoomProperty["State"] == "게임 중")            
                button.interactable = false;
            else
                button.interactable = true;
        }
    }

    private void OnDestroy()
    {       
        button.onClick.RemoveAllListeners();
    }

    public bool isContain(List<RoomInfo> roomList)
    {
        foreach(RoomInfo room in roomList)
        {
            if (CustomRoomProperty == room.CustomProperties)
                return true;
        }
        return false;
    }

}

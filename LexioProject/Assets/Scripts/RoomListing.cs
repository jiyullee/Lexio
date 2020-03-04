using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _roomNameText;
    private Text RoomNameText
    {
        get
        {
            return _roomNameText;
        }
    }
    [SerializeField]
    private Text _playerCountText;
    private Text PlayerCounttext
    {
        get
        {
            return _playerCountText;
        }
    }

    public string RoomName { get; private set; }
    public int MaxPlayerCount { get; private set; }
    public int CurrentPlayerCount { get; private set; }
    public bool Updated { get; set; }

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => LobbyManager.Instance.OnClickJoinRoom(RoomNameText.text));
    }

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void SetRoomNameText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName;
    }

    public void SetPlayerCountText(int current, int max)
    {
        CurrentPlayerCount = current;
        MaxPlayerCount = max;
        PlayerCounttext.text = $"{CurrentPlayerCount} / {MaxPlayerCount}";
    }

    private void Update()
    {
        
    }
}

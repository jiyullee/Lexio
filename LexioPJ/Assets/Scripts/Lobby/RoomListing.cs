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

    public void ChangeRoomInfo()
    {
        //방 번호 방 이름 방장이름 인원 배팅액 상황 고치기
    }
}

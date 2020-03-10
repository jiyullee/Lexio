using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lobby_MainCanvasManager : MonoBehaviour
{
    public static Lobby_MainCanvasManager Instance
    {
        get
        {
            instance = GameObject.FindObjectOfType<Lobby_MainCanvasManager>();
            if (instance == null)
                Debug.LogError("Can't find Instance");
            return instance;
        }
    }
    private static Lobby_MainCanvasManager instance;
    public Lobby_ChatService chatService;
    public Lobby_UserInfo UserInfo;
    public Lobby_CreateRoom lobby_CreateRoom;
    public Lobby_ChangeInfo lobby_ChangeInfo;
    public Lobby_GameRule lobby_GameRule;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            chatService.Input_OnEndEdit();
        }
    }

    public void Onclick_AppearCreateRoomPanel()
    {
        lobby_CreateRoom.gameObject.SetActive(true);
    }

    public void Onclick_AppearChangeInfoPanel()
    {
        lobby_ChangeInfo.gameObject.SetActive(true);
    }

    public void Onclick_AppearGameRuleBtn()
    {
        lobby_GameRule.gameObject.SetActive(true);
    }
}

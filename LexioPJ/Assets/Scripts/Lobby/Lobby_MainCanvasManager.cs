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
    public Lobby_Charge lobby_Charge;
    public Lobby_GameRule lobby_GameRule;
    public Lobby_OptionPanel lobby_OptionPanel;
    public QuitGame QuitPanel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            chatService.Input_OnEndEdit();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    private void Quit()
    {
        QuitPanel.gameObject.SetActive(true);
    }
    public void Onclick_AppearCreateRoomPanel()
    {
        lobby_CreateRoom.gameObject.SetActive(true);
    }

    public void Onclick_AppearChargePanel()
    {
        lobby_Charge.gameObject.SetActive(true);
    }

    public void Onclick_AppearGameRuleBtn()
    {
        lobby_GameRule.gameObject.SetActive(true);
        lobby_GameRule.nowIndex.text = "1 / 7";
        lobby_GameRule.panels[0].SetActive(true);
    }

    public void OnClick_AppearOptionPanel()
    {
        lobby_OptionPanel.gameObject.SetActive(true);
    }
}

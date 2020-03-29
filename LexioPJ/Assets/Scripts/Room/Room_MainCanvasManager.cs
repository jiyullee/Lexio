using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Room_MainCanvasManager : MonoBehaviourPun
{
    public static Room_MainCanvasManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Room_MainCanvasManager>();

            return instance;
        }
    }
    private static Room_MainCanvasManager instance;
    public Room_ChangeRoomInfo Room_ChangeRoomInfo;
    public Room_GameOption Room_GameOption;
    public Room_ChatService chatService;
    public QuitGame quitPanel;
    public Text RoomNameText;
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

    public void Onclick_AppearRoominfo()
    {
        Room_ChangeRoomInfo.gameObject.SetActive(true);
    }

    public void Onclick_AppearGameOption()
    {
        Room_GameOption.gameObject.SetActive(true);
    }

    private void Quit()
    {
        quitPanel.gameObject.SetActive(true);
    }

    public void ChangeRoomName(string name)
    {
        photonView.RPC("RPC_ChangeRoomNameText", RpcTarget.All, name);
    }
    [PunRPC]
    private void RPC_ChangeRoomNameText(string roomName)
    {
        RoomNameText.text = roomName;
    }
}

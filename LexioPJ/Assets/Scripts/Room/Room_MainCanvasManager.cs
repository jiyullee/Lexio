using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_MainCanvasManager : MonoBehaviour
{
    public Room_ChangeRoomInfo Room_ChangeRoomInfo;
    public Room_GameOption Room_GameOption;
    public Room_ChatService chatService;


    public void Onclick_AppearRoominfo()
    {
        Room_ChangeRoomInfo.gameObject.SetActive(true);
    }

    public void Onclick_AppearGameOption()
    {
        Room_GameOption.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            chatService.Input_OnEndEdit();
        }
    }
}

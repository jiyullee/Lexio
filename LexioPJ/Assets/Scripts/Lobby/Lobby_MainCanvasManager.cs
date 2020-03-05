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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            chatService.Input_OnEndEdit();
        }
    }

}

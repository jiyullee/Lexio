using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private GameObject _lobbyCanvas;
    private GameObject LobbyCanvas { get { return _lobbyCanvas; } }

    private void Start()
    {
        LobbyCanvas.transform.SetAsLastSibling();
    }
    public void GotoCurrentRoom()
    {
        LobbyCanvas.transform.SetAsFirstSibling();
    }
}

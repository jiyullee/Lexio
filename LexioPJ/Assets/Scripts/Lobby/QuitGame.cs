using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuitGame : MonoBehaviour
{
    public PlayerNetwork PlayerNetwork;
   
    public void Onclick_RoomQuit()
    {
        PlayerNetwork.QuitGame();
    }

    public void OnClick_CancelQuit()
    {
        if(SceneManager.GetActiveScene().name == "Lobby")
            Lobby_MainCanvasManager.Instance.buttonSound.Play();
        else if (SceneManager.GetActiveScene().name == "room")
            Room_MainCanvasManager.Instance.buttonSound.Play();
        
        gameObject.SetActive(false);
    }
    public void Onclick_Quit()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
            Lobby_MainCanvasManager.Instance.buttonSound.Play();
        else if (SceneManager.GetActiveScene().name == "Room")
            Room_MainCanvasManager.Instance.buttonSound.Play();
        Application.Quit();
    }
}

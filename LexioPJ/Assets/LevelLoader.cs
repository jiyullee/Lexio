using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviourPun
{
    public static LevelLoader Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<LevelLoader>();

            return instance;
        }
    }

    private static LevelLoader instance;
    public Animator transition;
    public float transitionTime = 2;
  
    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        if (sceneName == "Lobby")
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
}

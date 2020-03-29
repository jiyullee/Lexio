using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using System;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SoundManager>();

            return instance;
        }
    }
    Random r = new Random(Guid.NewGuid().GetHashCode());
    private static SoundManager instance;
    public AudioSource LobbyBackSound;
    public AudioSource RoomBackSound;
    public AudioSource GameBackSound;
    public AudioClip[] RoomBackSounds;
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Room")
        {
            RoomBackSound.clip = RoomBackSounds[r.Next(0, RoomBackSounds.Length)];
            RoomBackSound.Play();

        }
    }
    public void TurnOn_LobbyBackSound()
    {
        LobbyBackSound.Play();
    }

    public void TurnOff_LobbyBackSound()
    {
        LobbyBackSound.Pause();
    }

    public void TurnOn_RoomBackSound()
    {
        RoomBackSound.Play();
    }

    public void TurnOff_RoomBackSound()
    {
        RoomBackSound.Pause();
    }

    public void TurnOn_GameBackSound()
    {
        GameBackSound.Play();
    }

    public void TurnOff_GameBackSound()
    {
        GameBackSound.Pause();
    }
}

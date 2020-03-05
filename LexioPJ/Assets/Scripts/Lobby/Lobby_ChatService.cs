﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using ExitGames.Client.Photon;


public class Lobby_ChatService : MonoBehaviourPun, IChatClientListener
{
    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    public InputField inputField;
    public Text outputText;

    private void Start()
    {
        Application.runInBackground = true;
        //  userName = System.Environment.UserName;
        PhotonNetwork.LocalPlayer.NickName = "지율";
        userName = PhotonNetwork.LocalPlayer.NickName;
        currentChannelName = "Channel 001";

        chatClient = new ChatClient(this);
        chatClient.Connect("5f481764-8873-4402-a95c-7dfc0da35a4a", "1.0", new AuthenticationValues(userName));

        //AddLine(string.Format("연결시도", userName));
    }

    public void AddLine(string lineString)
    {
        outputText.text += lineString + "\r\n";
    }

    public void OnApplicationQuit()
    {
        if(chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
            Debug.LogError(message);
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
            Debug.LogWarning(message);
        else
            Debug.Log(message);
    }

    public void OnConnected()
    {
        
        chatClient.Subscribe(new string[] { currentChannelName }, 10);
    }
    
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnDisconnected()
    {
        AddLine("서버와의 연결이 끊어졌습니다.");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string s = "";
        for (int i = 0; i < messages.Length; i++)
        {
            s += messages[i];
        }
        AddLine(string.Format("{0} : {1}", senders[0], s.ToString()));
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine("Welcome to Lexio world!");
        //AddLine(string.Format("채널 입장({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널 퇴장({0})", string.Join(",", channels)));
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        chatClient.Service();

    }
    
    public void Input_OnEndEdit()
    {
        if (chatClient.State == ChatState.ConnectedToFrontEnd)
        {
            chatClient.PublishMessage(currentChannelName, inputField.text);
            inputField.text = "";
        }
    }
}

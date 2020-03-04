using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatService : MonoBehaviourPun
{
    public List<string> chatList = new List<string>();
    public Text chatBox;
    public InputField input;

    public void SendButton()
    {
        string currentMsg = "[" + PhotonNetwork.LocalPlayer.NickName + "]" + input.text;
        Send(RpcTarget.All, currentMsg);
        input.text = string.Empty;
    }

    void Send(RpcTarget _target, string _msg)
    {
        photonView.RPC("SendMsg", _target, _msg);
    }

    [PunRPC]
    void SendMsg(string _msg, PhotonMessageInfo _info)
    {
        AddChatBox(string.Format(_msg, "{0}: {1}"), _info.Sender);
    }

    void AddChatBox(string _msg, Photon.Realtime.Player sender)
    {
        string chat = chatBox.text;
        chat += string.Format("\n{0}", _msg);
        chatBox.text = chat;
        chatList.Add(_msg);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SendButton();
    }
}

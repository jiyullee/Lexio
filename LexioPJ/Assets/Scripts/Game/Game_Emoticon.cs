using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Emoticon : MonoBehaviourPun
{

    public void OnClick_SendEmoticon_Sleep()
    {
        string msg = "빨리";
        string name = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("RPC_SendEmoticon", RpcTarget.All, msg, name);
    }
    public void OnClick_SendEmoticon_Mad()
    {
        string msg = "화남";
        string name = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("RPC_SendEmoticon", RpcTarget.All, msg, name);
    }
    public void OnClick_SendEmoticon_Disappoint()
    {
        string msg = "실망";
        string name = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("RPC_SendEmoticon", RpcTarget.All, msg, name);
    }
    public void OnClick_SendEmoticon_Love()
    {
        string msg = "행복";
        string name = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("RPC_SendEmoticon", RpcTarget.All, msg, name);
    }

    [PunRPC]
    private void RPC_SendEmoticon(string msg, string name)
    {
        PlayerScript[] players = FindObjectsOfType<PlayerScript>();
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].photonView.Owner.NickName == name)
            {
                players[i].OnEmoticon(msg);
            }
        }
    }
}

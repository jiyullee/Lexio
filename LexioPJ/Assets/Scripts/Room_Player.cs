using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Room_Player : MonoBehaviourPun
{
    public Text nameText;
    public GameObject masterObj;
    void Start()
    {
        photonView.RPC("RPC_Master", RpcTarget.All);
    }

    public void SetNameText(string str)
    {
        nameText.text = str;
    }
    
    [PunRPC]
    private void RPC_Master()
    {
        if (photonView.Owner.IsMasterClient)
        {
            masterObj.SetActive(true);
        }
        else
        {
            masterObj.SetActive(false);
        }
    }

    public void LeftRoom(Player player)
    {
        if(photonView.Owner == player)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }
}

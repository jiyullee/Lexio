using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Room_Player : MonoBehaviourPun
{
    public GameObject masterObj;
    
    public void OnMasterSign()
    {
        if (photonView.Owner.IsMasterClient)
        {
            photonView.RPC("RPC_Master", RpcTarget.All, true);
        }
        else
        {
            photonView.RPC("RPC_Master", RpcTarget.All, false);
        }
    }
    public void LeftRoom(Player player)
    {
        if(photonView.Owner == player)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }

    [PunRPC]
    private void RPC_Master(bool isMaster)
    {
        masterObj.SetActive(isMaster);
    }
}

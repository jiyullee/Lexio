using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_MainCanvasManager : MonoBehaviour
{
    public Room_ChangeRoomInfo Room_ChangeRoomInfo;

    public void Onclick_AppearRoominfo()
    {
        Room_ChangeRoomInfo.gameObject.SetActive(true);
    }
}

using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
public class RoomLayoutGroup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject RoomListingPrefab
    {
        get
        {
            return _roomListingPrefab;
        }
    }
    private List<RoomListing> _roomListingButtons { get; set; } = new List<RoomListing>();
    private List<RoomListing> RoomListingButtons
    {
        get { return _roomListingButtons; }
    }
    public Lobby_CreateRoom Lobby_CreateRoom;

    private List<string> RoomNames = new List<string>();
    List<RoomListing> removeRooms = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (!room.RemovedFromList)
                RoomReceived(room);            
        }
        foreach (RoomListing roomListing in RoomListingButtons)
        {
            if (!roomListing.isContain(roomList))
            {
                removeRooms.Add(roomListing);
            }
        }
        RemoveOldRooms();
    }

    
    private void RoomReceived(RoomInfo room)
    {
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);
        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);
            }

        }
        if (index != -1)
        {
            RoomListing roomListing = RoomListingButtons[index];
            roomListing.CustomRoomProperty = room.CustomProperties;
            roomListing.room = room;
            RoomNames.Add(roomListing.RoomNameText.text);
            roomListing.Updated = true;
        }
    }

    private void RemoveOldRooms()
    {

        foreach (RoomListing roomListing in removeRooms)
        {
            RoomListingButtons.Remove(roomListing);
            Destroy(roomListing.gameObject);
        }
        removeRooms.Clear();

    }

}

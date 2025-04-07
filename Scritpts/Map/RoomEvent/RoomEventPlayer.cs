using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomEventPlayer : MonoBehaviour
{
    private List<IRoomEvent> roomEvents;
     
    private void Awake()
    {
        roomEvents = GetComponents<IRoomEvent>().ToList();
    }
     
    public void StartEvents()
    {
        roomEvents.ForEach(x => x.StartRoomEvent());
    }
    public bool IsEndEvents()
    {
        return roomEvents.All(x => x.EndRoomEvent());
    }
}

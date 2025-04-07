using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMinimap : MonoBehaviour
{
    private Room _room;
    private SpriteRenderer _roomMinimapSpriteRenderer;
    //private SpriteRenderer[] _roomBridgesSpriteRenderers = new SpriteRenderer[4];
    private bool _isVisited = false;

    private void Awake()
    {
        _room = GetComponentInParent<Room>();
        _roomMinimapSpriteRenderer = GetComponent<SpriteRenderer>();
        _roomMinimapSpriteRenderer.enabled = false;
        _roomMinimapSpriteRenderer.color = Color.gray;
    }

    private void Update()
    {
        if (_isVisited == false && Room.currentPlayerInRoom == _room)
        {
            _roomMinimapSpriteRenderer.enabled = true;
            _roomMinimapSpriteRenderer.color = Color.white;

            for(int i =0;i<_room.bridges.Length;++i)
            {
                if(_room.bridges[i] != null)
                {
                    _room.bridges[i].Find("BridgeMinimap").gameObject.SetActive(true);
                    _room.nearRooms[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
                }
            }

            _isVisited = true;
        }
    }

}

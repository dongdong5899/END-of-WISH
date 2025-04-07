using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    static public Room currentPlayerInRoom;
    static public Room currentCameraInRoom;

    private Vector3Int intPosition;

    [SerializeField] private LayerMask whatIsPlayer;

    private Animator[] animators;
    private MeshRenderer[] renderers;
    public Transform[] bridges = new Transform[4];
    public Room[] nearRooms = new Room[4];
    private readonly int openAnimationHash = Animator.StringToHash("Open");
    private readonly int closeAnimationHash = Animator.StringToHash("Close");

    [SerializeField] private int _damage = -10;

    public Vector4 doorDir;
    public event Action RoomEndEvent;

    private bool isCodeStarted = false;
    private bool isCleared = false;

    private List<RoomEventPlayer> roomEvents;

    public void Initialize(Vector3Int position)
    {
        intPosition = position;


        roomEvents = GetComponentsInChildren<RoomEventPlayer>().ToList();

        animators = transform.GetComponentsInChildren<Animator>();
        renderers = transform.GetComponentsInChildren<MeshRenderer>();
        GameManager.Instance.DelayFrameCallback(2, () =>
        {
            RenderActive(false);
            for (int i = 0; i < 4; i++)
            {
                if (doorDir[i] > 0)
                {
                    animators[i].SetTrigger("StartOpen");
                    animators[i].GetComponent<BoxCollider>().enabled = false;
                }
            }
            isCodeStarted = true;
        });
    }

    private void Update()
    {
        if (isCodeStarted == false) return;

        if (Physics.BoxCast(transform.position + Vector3.down, new Vector3(25, 0.2f, 25), Vector3.up, Quaternion.identity, 5, whatIsPlayer))
        {
            if (currentPlayerInRoom != this)
            {
                currentPlayerInRoom = this;
                StartCoroutine(ActiveRoom(1));
            }
        }
        Vector3 cameraPos = CameraManager.Instance.CurrentVCam.transform.position;

        if (Mathf.Abs(cameraPos.x - transform.position.x) < 30 &&
            Mathf.Abs(cameraPos.z - transform.position.z) < 30 &&
            transform.position.y - cameraPos.y < 0 && transform.position.y - cameraPos.y > -30)
        {
            if (currentCameraInRoom != this)
            {
                if (currentCameraInRoom != null) currentCameraInRoom.NearRoomRenderingActive(false);
                currentCameraInRoom = this;
                NearRoomRenderingActive(true);
            }
        }
    }

    private IEnumerator ActiveRoom(float startDelay)
    {
        if (isCleared) yield break;
        if (roomEvents.Count == 0)
        {
            MapMaker.ClearCount++;
            isCleared = true;

            Debug.Log(MapMaker.ClearCount);
            yield break;
        }


        DoorOpen(false);

        yield return new WaitForSeconds(startDelay);

        foreach (RoomEventPlayer roomEvent in roomEvents)
        {
            roomEvent.StartEvents();

            yield return new WaitUntil(roomEvent.IsEndEvents);
            isCleared = true;
        }
        MapMaker.ClearCount++;
        Debug.Log(MapMaker.ClearCount);
        RoomEndEvent?.Invoke();
        DoorOpen(true);
    }

    public void DoorOpen(bool isOpen)
    {
        GameManager.Instance.player.ApplyDamage(_damage, Vector3.zero, Vector3.zero,0,0,null);
        for (int i = 0; i < 4; i++)
        {
            if (doorDir[i] > 0)
            {
                animators[i].SetTrigger(isOpen ? openAnimationHash : closeAnimationHash);
                animators[i].GetComponent<BoxCollider>().enabled = !isOpen;
            }
        }
    }

    public void NearRoomRenderingActive(bool value)
    {
        for (int i = 0; i < 4; i++)
        {
            if (doorDir[i] != 0)
            {
                NearRoomRenderLink(value, i);
            }
        }
    }

    public void NearRoomRenderLink(bool value, int dir = -1)
    {
        RenderActive(value);
        if (dir != -1)
        {
            if (MapMaker.PosToRoomDictionary.TryGetValue
                (Vector3Int.CeilToInt(MapMaker.YToZ(MapMaker.VectorDirs[dir]) + intPosition), out Room room)
                && doorDir[dir] != 0)
            {
                room.NearRoomRenderLink(value, dir);
            }
        }
    }

    public void RenderActive(bool value)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].enabled = value;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (bridges[i] != null)
            {
                bridges[i].GetComponent<MeshRenderer>().enabled = value;
            }
        }
    }
}

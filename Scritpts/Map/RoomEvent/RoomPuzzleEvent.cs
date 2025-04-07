using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomPuzzleEvent : MonoBehaviour, IRoomEvent
{
    [SerializeField] private List<SoundPuzzle> metryPuzzle;
    private bool _isClear;

    private void Awake()
    {
        if (metryPuzzle.Count > 0)
        {
            //metryPuzzle.ForEach(x => x)
        }
    }

    public void StartRoomEvent()
    {
        //GameManager.Instance.SetCursorActive(true);
        metryPuzzle.ForEach(x => x.PassWordSet());
        Debug.Log("퍼즐 방 이벤트 발생");
    }

    public bool EndRoomEvent()
    {
        //GameManager.Instance.SetCursorActive(false);
        if (_isClear == true) 
            return true;
        Debug.Log("클리어이벤트");
        return _isClear;
    }
}

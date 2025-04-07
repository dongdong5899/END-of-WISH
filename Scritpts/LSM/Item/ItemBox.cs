using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{

    [SerializeField] private Item _item;
    private Room _room;
    private bool _openBox;
    private bool _closeBox;
    public Animator Animator { get;private set; }

    private void Awake()
    {
        _room = transform.root.GetComponent<Room>();
        Animator = GetComponent<Animator>();
        //_item = GetComponentInChildren<Item>();
        if(_room != null)
            _room.RoomEndEvent += OpenItemBox;
        Instantiate(_item, transform);
    }

    private void OnDestroy()
    {
        if (_room != null)
            _room.RoomEndEvent -= OpenItemBox;
    }

    private void OpenItemBox()
    {
        Animator.SetTrigger("Open");
        Item ChipItem =  Instantiate(_item, transform);
        //ChipItem.transform. = Vector3.one * 0.1f;
    }
}

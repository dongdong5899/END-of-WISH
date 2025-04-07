using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action<Vector3> MovementEvent;
    public Action<Vector2> MouseMoveEvent;
    public Action SpaceEvent;
    public Action AttackEvent;

    private void FixedUpdate()
    {
        CheckMoveInput();
    }

    private void Update()
    {
        CheckSpaceInput();
        if (GameManager.Instance.CanPlayerMouseControl)
        {
            CheckAttackInput();
            CheckMouseMoveInput();
        }
    }

    private void CheckMouseMoveInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        Vector2 mouseMove = new Vector2(-mouseY, mouseX);

        MouseMoveEvent?.Invoke(mouseMove);
    }

    private void CheckAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttackEvent?.Invoke();
        }
    }

    private void CheckSpaceInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceEvent?.Invoke();
        }
    }

    private void CheckMoveInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveY);

        MovementEvent?.Invoke(movement.normalized);
    }
}

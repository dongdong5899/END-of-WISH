using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerBrain player;

    [SerializeField] private float gravity;

    [SerializeField] private float cameraClamp;

    private float verticalVelocity;
    private bool _isDie = false;
    private Vector3 velocity;

    public bool IsGround => characterController.isGrounded;
    private Quaternion targetRotation;

    public void Initialize()
    {
        characterController = GetComponent<CharacterController>();
        player = GetComponent<PlayerBrain>();
        player.InputCompo.MouseMoveEvent += HandleRotationEvent;
    }

    public void Die()
    {
        _isDie = true;
        characterController.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_isDie) return;

        Gravity();
        Rotation();
        Move();
    }

    private void Rotation()
    {
        float rotateSpeed = 8f;
        player.visualTrm.rotation = Quaternion.Lerp(player.visualTrm.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
    }

    private float mouseMoveX;
    private float mouseMoveY;
    private void HandleRotationEvent(Vector2 mouseMove)
    {
        mouseMoveX += mouseMove.x;
        mouseMoveY += mouseMove.y;
        mouseMoveX = Mathf.Clamp(mouseMoveX, -cameraClamp / 2, cameraClamp / 2);
        player.rotationTrm.eulerAngles = new Vector3(mouseMoveX, mouseMoveY, 0);
    }

    public void SetInputRotation()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX == 0 && moveY == 0) return;

        Vector3 movement = player.rotationTrm.right * moveX + 
            Vector3.ProjectOnPlane(player.rotationTrm.forward, Vector3.up) * moveY;

        SetRotation(movement);
    }

    private void Move()
    {
        characterController.Move(velocity);
    }

    public void SetMovement(Vector3 movement, bool isApplyRotation)
    {
        velocity = movement * Time.fixedDeltaTime;

        if (velocity.sqrMagnitude > 0 && isApplyRotation)
        {
            targetRotation = Quaternion.LookRotation(velocity);
        }
    }

    public void SetRotation(Vector3 lookDir)
    {
        targetRotation = Quaternion.LookRotation(lookDir);
        player.visualTrm.rotation = targetRotation;
    }

    public void MoveStop()
    {
        velocity = Vector3.zero;
    }

    public void SetJump(float jumpPower)
    {
        verticalVelocity = jumpPower;
    }

    private void Gravity()
    {
        if (IsGround && verticalVelocity <= 0)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }
        velocity.y = verticalVelocity;
    }
}

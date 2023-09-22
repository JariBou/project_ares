using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 6f;
    private Rigidbody2D _rb;

    public Vector2 MoveVector { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>().normalized * _speed;
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
    }

    private void OnEnable()
    {
        TickManager.FrameUpdate += OnFrameUpdate;
    }

    private void OnFrameUpdate()
    {
        _rb.velocity = new Vector2(MoveVector.x, _rb.velocity.y);
    }

    private void OnDisable()
    {
        TickManager.FrameUpdate -= OnFrameUpdate;
    }
}

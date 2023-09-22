using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private float _speed;
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

    private void OnEnable()
    {
        TickManager.FrameUpdate += OnFrameUpdate;
    }

    private void OnFrameUpdate()
    {
        Debug.Log($"MoveVector: x={MoveVector.x}, y={MoveVector.y}");
        _rb.velocity = MoveVector;
    }

    private void OnDisable()
    {
        TickManager.FrameUpdate -= OnFrameUpdate;
    }
}

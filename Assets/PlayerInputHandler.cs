using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : NetworkBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    [Client(RequireOwnership = true)]
    public void Move(InputAction.CallbackContext context)
    {
        _rb.velocity = context.ReadValue<Vector2>().normalized * _speed;
    }
}

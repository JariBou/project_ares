using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{
    private int _playerId;
    public void SetPlayerId(int configuration) => _playerId = configuration;

    [SerializeField] private ColorSelection _colorSelectionSo;
    private Image _image;
    private int _selectionIndex;

    private bool _isReady;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _image.color = _colorSelectionSo._possibleColors[0];
    }
    
    /* TODO: Clicking on a button that is assigned triggers it but not visually for some reason
     Possible solution: Manually assign callbacks after player is spawned 
    */ 
    public void Next(InputAction.CallbackContext context)
    {
        if(!context.performed || _isReady) return;
        _selectionIndex = (_selectionIndex + 1) % _colorSelectionSo._possibleColors.Count;

        PlayerManager.Instance.GetPlayerConfigs()[_playerId].SelectionIndex = _selectionIndex;
        _image.color = PlayerManager.Instance.GetPlayerColor(_selectionIndex);
    }

    public void Previous(InputAction.CallbackContext context)
    {
        if(!context.performed || _isReady) return;
        _selectionIndex = _selectionIndex == 0 ? _colorSelectionSo._possibleColors.Count - 1 : _selectionIndex - 1;
        
        PlayerManager.Instance.GetPlayerConfigs()[_playerId].SelectionIndex = _selectionIndex;
        _image.color = PlayerManager.Instance.GetPlayerColor(_selectionIndex);
    }

    public void Confirm(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        Debug.Log($"Readying Player {_playerId}");
        PlayerManager.Instance.ReadyPlayer(_playerId);
        _isReady = true;
    }
    
    
}

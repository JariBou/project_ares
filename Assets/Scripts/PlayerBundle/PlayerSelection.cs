using System;
using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.PlayerBundle
{
    public class PlayerSelection : MonoBehaviour
    {
        private int _playerId;
        public void SetPlayerId(int configuration) => _playerId = configuration;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _typeText;
        [SerializeField] private TMP_Text _playerNameText;
        private PlayerManager _playerManager;
    
        private int _selectionIndex;

        private bool _isReady;
        private bool _isActive;

        private void Awake()
        {
            _playerManager = GameObject.FindWithTag("Managers").GetComponent<PlayerManager>();
            _playerNameText.text = $"Player {_playerId}";
            _playerNameText.color = _playerManager.GetColorOfPlayer(_playerId);
        }

        private void Start()
        {
            UpdateDisplay();
        }
    
        private void OnEnable()
        {
            TickManager.FrameUpdate += OnFrameUpdate;
        }
    
        private void OnDisable()
        {
            TickManager.FrameUpdate -= OnFrameUpdate;
        }

        private void UpdateDisplay()
        {
            Character currChar = _playerManager.CharacterManager[_selectionIndex];
            _nameText.text = $"Name: {currChar._name}";
            _typeText.text = $"Type: {Enum.GetName(typeof(CharacterType), currChar._characterType)}";
        }
    
        // This makes it so that Inputs are only processed after the player appears on screen
        // Because HandlePlayerJoin is called before Awake and Before Start
        private void OnFrameUpdate()
        {
            if (!_isActive) _isActive = true;
        }
        
        public void Next(InputAction.CallbackContext context)
        {
            if(!context.performed || _isReady || !_isActive) return;
            _selectionIndex = (_selectionIndex + 1) % PlayerManager.Instance.CharacterManager.Count;

            PlayerManager.Instance.PlayerConfigs[_playerId].SelectionIndex = _selectionIndex;
            UpdateDisplay();
        }

        public void Previous(InputAction.CallbackContext context)
        {
            if(!context.performed || _isReady || !_isActive) return;
            _selectionIndex = _selectionIndex == 0 ? PlayerManager.Instance.CharacterManager.Count - 1 : _selectionIndex - 1;
            PlayerManager.Instance.PlayerConfigs[_playerId].SelectionIndex = _selectionIndex;
            UpdateDisplay();
        }

        public void Test(InputAction.CallbackContext context)
        {
            if (!context.performed) {return;}
            EffectsManager.StartShockwave(Vector2.zero, 1f);
        }

        public void Confirm(InputAction.CallbackContext context)
        {
            if(!context.performed || !_isActive) return;
            Debug.Log($"Readying Player {_playerId}");
            PlayerManager.Instance.ReadyPlayer(_playerId);
            _isReady = true;
        }
    
    
    }
}

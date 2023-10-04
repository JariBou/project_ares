using System;
using Core;
using JetBrains.Annotations;
using NaughtyAttributes;
using ProjectAres.Managers;
using ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField, CanBeNull] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private HurtBoxesManager _hurtBoxesManager;

        [SerializeField] private TMP_Text _text;
        [SerializeField, Expandable] private Character _character;
        [SerializeField] private Animator _animator;
        private Rigidbody2D _rb;
        public int PlayerId { get; private set; }
        public Animator Animator => _animator;
        public Character Character => _character;

        private int _iFramesCount;
        private int _blockedFramesCount;
        public bool IsInvincible { get; private set; }
        
        #if UNITY_EDITOR
        [SerializeField] private bool _overrideCharacterValues;
        [SerializeField] private Vector2 _groundCheckOffset;
        [SerializeField] private Vector2 _groundCheckSize;
        #endif
        [SerializeField]private BoxCollider2D _groundDetector;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _groundDetector = GetComponent<BoxCollider2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _text.text = _character._name;
            // For now dummy also has a playerCharacter soooooo, i need to do this
            if (_playerInputHandler != null) _playerInputHandler.SetCharacterStats(_character);
            _hurtBoxesManager.SetOwners(this);
            _groundDetector.offset = _character._groundCheckOffset;
            _groundDetector.size = _character._groundCheckSize;
        }

        public PlayerCharacter WithCharacter(int playerId)
        {
            PlayerId = playerId;
            _character = PlayerManager.Instance.GetCharacterOfPlayer(playerId);
            return this;
        }
        
        public PlayerCharacter SetId(int playerId)
        {
            PlayerId = playerId;
            return this;
        }
        
        public PlayerCharacter SetCharacter(Character character)
        {
            _character = character;
            return this;
        }

       
        
        private void OnPreUpdate()
        {
            IsInvincible = _iFramesCount > 0;
            if (_playerInputHandler != null) _playerInputHandler.SetMoveState(_blockedFramesCount == 0);
            _iFramesCount = _iFramesCount > 0 ? _iFramesCount-1 : 0;
            _blockedFramesCount = _blockedFramesCount > 0 ? _blockedFramesCount-1 : 0;
        }

        public void SetIFrames(int frameCount)
        {
            _iFramesCount = frameCount;
            IsInvincible = _iFramesCount > 0;
        }

        public void SetBlockedFramesCount(int frameCount)
        {
            _blockedFramesCount = frameCount;
            if (_playerInputHandler != null) _playerInputHandler.SetMoveState(_blockedFramesCount == 0);
        }

        // TODO: Should combine SetBlockedFrames And SetIFrames
        public void IsAttacked()
        {
            if (_playerInputHandler != null) _playerInputHandler.StoppedAttacking();
        }
        
        public void ApplyKb(Vector2 force)
        {
            Debug.Log($"Force Before Compensation: {force}");
            force.y *= SpeedCompensationFunction(-_rb.velocity.y);
            Debug.Log($"Force After Compensation: {force}");
            
            _rb.AddForce(force, ForceMode2D.Impulse);
        }

        private float SpeedCompensationFunction(float x)
        {
            return 0.15f * x + 1;
            return (float)(0.4f * (Math.Exp(0.3f * x) - 1) + 1);
        }

        private void OnEnable()
        {
            TickManager.PreUpdate += OnPreUpdate;
        }

        private void OnDisable()
        {
            TickManager.PreUpdate -= OnPreUpdate;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_overrideCharacterValues)
            {
                _character._groundCheckOffset = _groundCheckOffset;
                _character._groundCheckSize = _groundCheckSize;
            }
            _groundDetector.offset = _groundCheckOffset;
            _groundDetector.size = _groundCheckSize;
        }
        #endif

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            if (_playerInputHandler != null) _playerInputHandler.SetGrounded(true);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            if (_playerInputHandler != null) _playerInputHandler.SetGrounded(true);
        }
        
    }
}

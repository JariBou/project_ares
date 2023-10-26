using System;
using NaughtyAttributes;
using ProjectAres.Managers;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class PlayerCharacter : Damageable
    {
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        public PlayerInputHandler InputHandler => _playerInputHandler;
        
        #if UNITY_EDITOR
        [SerializeField, Foldout("Ground Check")] private Vector2 _groundCheckOffset;
        [SerializeField, Foldout("Ground Check")] private Vector2 _groundCheckSize;
        [SerializeField, Foldout("Ground Check")] private bool _showGroundDetection;
        [SerializeField, Foldout("Ground Check"), ShowIf("_showGroundDetection")] private Color _groundCheckColor;
        #endif
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerManager = GameObject.FindWithTag("Managers").GetComponent<PlayerManager>();
            StartPos = transform.position;
        }

        void Start()
        {
            _text.text = _playerManager.DisplayCharacterName ? _character._name : $"Player {PlayerId}";
            _text.color = _playerManager.GetColorOfPlayer(PlayerId);
            _hurtBoxesManager.SetOwners(this); 
            InputHandler.SetCharacter(_character);
        }

        protected override void OnPostUpdate()
        {
            base.OnPostUpdate();
            InputHandler.SetMoveState(BlockedFramesCount == 0);
        }
        
        public override void SetBlockedFramesCount(int frameCount)
        {
            base.SetBlockedFramesCount(frameCount);
            InputHandler.SetMoveState(BlockedFramesCount == 0);
        }

        // TODO: Should combine SetBlockedFrames And SetIFrames
        public override void IsAttacked(float damage)
        {
            InputHandler.StopAttacking();
            PlayerHealth -= damage;
        }

       

        #if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                _character._groundCheckOffset = _groundCheckOffset;
                _character._groundCheckSize = _groundCheckSize;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        private void OnDrawGizmos()
        {
            if (_showGroundDetection)
            {
                Gizmos.color = _groundCheckColor;
                Gizmos.DrawWireCube((Vector2)transform.position + _groundCheckOffset, _groundCheckSize);
            }
        }
        #endif
        
    }
}

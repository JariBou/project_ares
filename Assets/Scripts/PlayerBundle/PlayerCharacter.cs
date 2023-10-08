using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class PlayerCharacter : Damageable
    {
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        
        #if UNITY_EDITOR
        [SerializeField, Foldout("Ground Check")] private Vector2 _groundCheckOffset;
        [SerializeField, Foldout("Ground Check")] private Vector2 _groundCheckSize;
        [SerializeField, Foldout("Ground Check")] private bool _showGroundDetection;
        [SerializeField, Foldout("Ground Check"), ShowIf("_showGroundDetection")] private Color _groundCheckColor;
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        public PlayerInputHandler InputHandler => _playerInputHandler;
#endif
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        void Start()
        {
            _text.text = _character._name;
            _hurtBoxesManager.SetOwners(this);
            InputHandler.SetCharacter(_character);
        }

        protected override void OnPreUpdate()
        {
            base.OnPreUpdate();
            InputHandler.SetMoveState(BlockedFramesCount == 0);
        }
        
        public override void SetBlockedFramesCount(int frameCount)
        {
            base.SetBlockedFramesCount(frameCount);
            InputHandler.SetMoveState(BlockedFramesCount == 0);
        }

        // TODO: Should combine SetBlockedFrames And SetIFrames
        public override void IsAttacked()
        {
            Debug.Log("I was attacked!!!");
            InputHandler.StopAttacking();
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            _character._groundCheckOffset = _groundCheckOffset;
            _character._groundCheckSize = _groundCheckSize;
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

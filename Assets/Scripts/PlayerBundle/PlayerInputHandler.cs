using System;
using Core;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.PlayerBundle
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private Rigidbody2D _rb;
        public Vector2 _groundCheckOffset;
        public Vector2 _groundCheckSize;
        private Animator _animator;
        private bool _isAttacking;
        private float _gravitySave;
        private bool _isFlipped;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int YSpeed = Animator.StringToHash("ySpeed");
        [SerializeField] private int _baseNumberOfJumps = 2; // Might be set in character, can be passed from PlayerCharacter.cs
        private int _numberOfJumps;
        private bool _hasJumped;
        public bool CanMove { get; private set; }

        private Vector2 MoveVector { get; set; }
        private bool _isGrounded;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _gravitySave = _rb.gravityScale;
            _numberOfJumps = _baseNumberOfJumps;
        }
    
        public void Move(InputAction.CallbackContext context)
        {
            Vector2 moveVecNormalized = context.ReadValue<Vector2>().normalized;
            MoveVector = context.ReadValue<Vector2>().normalized * _speed;
            if (!CanMove) return;
            
            if (moveVecNormalized.y > 0.95f)
            {
                if (!_hasJumped) Jump(context);
            }
            else
            {
                _hasJumped = false;
            }
            
            _isFlipped = moveVecNormalized.x switch
            {
                < 0 => false,
                > 0 => true,
                _ => _isFlipped
            };
        }
    
        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking || _numberOfJumps==0) {return;}

            Vector2 rbVelocity = _rb.velocity;
            rbVelocity = new Vector2(rbVelocity.x, _jumpForce);
            _rb.velocity = rbVelocity;
            _numberOfJumps--;
            _animator.SetFloat(YSpeed, Math.Abs(rbVelocity.y));
            _hasJumped = true;
        }

        public void ButtonSouth(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking) {return;}
            EffectsManager.StartShockwave(transform.position, 1f);
        }
        
        public void ButtonEast(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking) {return;}
            _animator.SetTrigger("Attack");
        }
        
        private void OnFrameUpdate()
        {
            if (_isAttacking)
            {
                _rb.velocity = Vector2.zero;
                _animator.SetFloat(Speed, 0);
            }
            else
            {
                // Ok so, rn players can only take vertical KB, possible solution: via animator when hurt use a bool
                // Or: maybe the best, in _onPreFrameActions we chan check if the player's uncontrolable frames are at 0
                // smth like that u get it right?
                if (CanMove)
                {
                    _rb.velocity = new Vector2(MoveVector.x, _rb.velocity.y);
                    _animator.SetFloat(Speed, Math.Abs(MoveVector.x));
                    _animator.SetFloat(YSpeed, Math.Abs(_rb.velocity.y));
                }
            }
            // TODO: Rotation also rotates nameplate, needs fixing (look at PlayerTest 3.prefab)
            transform.rotation = Quaternion.Euler(new Vector3(0, _isFlipped ? 180 : 0, 0));
        }

        public void SetMoveState(bool status)
        {
            CanMove = status;
        }

        public void StartedAttacking()
        {
            _isAttacking = true;
            _rb.gravityScale = 0f;
        }
        
        public void StoppedAttacking()
        {
            _isAttacking = false;
            _rb.gravityScale = _gravitySave;
        }

        // TODO: Implement ActionStack to define actions made in each frame
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EffectsManager.SpawnHitParticles(other.GetContact(0).point);
            }
            
        }

        private void OnEnable()
        {
            TickManager.FrameUpdate += OnFrameUpdate;
        }
     
        private void OnDisable()
        {
            TickManager.FrameUpdate -= OnFrameUpdate;
        }

        public void SetCharacterStats(Character character)
        {
            _speed = character._speed;
            _jumpForce = character._jumpForce;
            _groundCheckOffset = character._groundCheckOffset;
            _groundCheckSize = character._groundCheckSize;
        }


        public void SetGrounded(bool state)
        {
            _numberOfJumps = _baseNumberOfJumps + (state ? 0 : -1);
            _isGrounded = state;
        }
    }
}

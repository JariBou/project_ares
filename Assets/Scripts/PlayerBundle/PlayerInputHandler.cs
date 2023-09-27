using System;
using Core;
using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.PlayerBundle
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private Rigidbody2D _rb;
        private Animator _animator;
        private bool _isAttacking;
        private float _gravitySave;
        private bool _isFlipped;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private Vector2 MoveVector { get; set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _gravitySave = _rb.gravityScale;
        }
    
        public void Move(InputAction.CallbackContext context)
        {
            MoveVector = context.ReadValue<Vector2>().normalized * _speed;
            _isFlipped = MoveVector.x switch
            {
                < 0 => false,
                > 0 => true,
                _ => _isFlipped
            };
        }
    
        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking) {return;}
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }

        public void ButtonSouth(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking) {return;}
            EffectsManager.StartShockwave(transform.position, 1f);
        }
        
        public void ButtonEast(InputAction.CallbackContext context)
        {
            if (!context.performed) {return;}
            _animator.SetTrigger("Attack");
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

        private void OnFrameUpdate()
        {
            if (_isAttacking)
            {
                _rb.velocity = Vector2.zero;
                _animator.SetFloat(Speed, 0);
            }
            else
            {
                _rb.velocity = new Vector2(MoveVector.x, _rb.velocity.y);
                _animator.SetFloat(Speed, Math.Abs(MoveVector.x));
            }
            // TODO: Rotation also rotates nameplate, needs fixing (look at PlayerTest 3.prefab)
            transform.rotation = Quaternion.Euler(new Vector3(0, _isFlipped ? 180 : 0, 0));
        }

        private void OnDisable()
        {
            TickManager.FrameUpdate -= OnFrameUpdate;
        }

        public void SetCharacterStats(Character character)
        {
            _speed = character._speed;
            _jumpForce = character._jumpForce;
        }
    }
}

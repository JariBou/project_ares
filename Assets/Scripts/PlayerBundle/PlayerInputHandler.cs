using System;
using System.Collections.Generic;
using ProjectAres.Core;
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
        public Vector2 _groundCheckOffset;
        public Vector2 _groundCheckSize;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
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
        
        [Header("Combo System")]
        private Character _character;

        private int _comboCount;

        private List<ButtonPos> _prevInputs;
        private int _lastAttackFrameAge;
        [SerializeField]private int _maxLastAttackFrameAge = 5;
        private bool _canAttack = true;
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int Attack = Animator.StringToHash("Attack");
        public AttackSo CurrentAttack { get; private set; }

        private Vector3 _targetPos;
        [SerializeField] private PlayerCharacter _playerCharacter;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _gravitySave = _rb.gravityScale;
            _numberOfJumps = _baseNumberOfJumps;
            _prevInputs = new List<ButtonPos>(5);
        }
    
        public void Move(InputAction.CallbackContext context)
        {
            Vector2 moveVecNormalized = context.ReadValue<Vector2>().normalized;
            MoveVector = moveVecNormalized * _speed;
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
                < 0 => _character._isFacingRight,
                > 0 => !_character._isFacingRight,
                _ => _isFlipped
            };
        }
    
        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking || _numberOfJumps==0) {return;}
            if (!CanMove) return;

            Vector2 rbVelocity = _rb.velocity;
            rbVelocity = new Vector2(rbVelocity.x, _jumpForce);
            _rb.velocity = rbVelocity;
            _numberOfJumps--;
            _animator.SetFloat(YSpeed, rbVelocity.y);
            _hasJumped = true;
        }

        public void ButtonSouth(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking || !_canAttack) {return;}
            EffectsManager.StartShockwave(transform.position, 1f);
        }
        
        public void TriggerR2(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking || !_canAttack) {return;}
            
            ProcessAttack(ButtonPos.TriggerR2);
            // _animator.runtimeAnimatorController = _character._attackCombos[0].Attacks[_comboCount]._animatorOverride;
            // _comboCount = (_comboCount + 1) % _character._attackCombos[0].Count;
            // _animator.SetTrigger("Attack");
        }
        
        public void ButtonEast(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking || !_canAttack) {return;}
            
            ProcessAttack(ButtonPos.East);
            // _animator.runtimeAnimatorController = _character._attackCombos[0].Attacks[_comboCount]._animatorOverride;
            // _comboCount = (_comboCount + 1) % _character._attackCombos[0].Count;
            // _animator.SetTrigger("Attack");
        }
        
        public void ButtonNorth(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking || !_canAttack) {return;}

            ProcessAttack(ButtonPos.North);
            // _animator.runtimeAnimatorController = _character._attackCombos[0].Attacks[_comboCount]._animatorOverride;
            // _comboCount = (_comboCount + 1) % _character._attackCombos[0].Count;
            // _animator.SetTrigger("Attack");
        }
        
        public void ButtonWest(InputAction.CallbackContext context)
        {
            if (!context.performed || _isAttacking || !_canAttack) {return;}

            ProcessAttack(ButtonPos.West);
            // _animator.runtimeAnimatorController = _character._attackCombos[0].Attacks[_comboCount]._animatorOverride;
            // _comboCount = (_comboCount + 1) % _character._attackCombos[0].Count;
            // _animator.SetTrigger("Attack");
        }

        private void ProcessAttack(ButtonPos inputButtonPos)
        {
            _prevInputs.Add(inputButtonPos);
            bool isInCombo = _character.GetCurrentComboAttack(_prevInputs, _comboCount, out AttackSo attack);
            // AttackSo attack = _character.GetCurrentComboAttack(_prevInputs, _comboCount);
            
            Debug.Log($"IsCombo = {isInCombo}");
            
            if (_comboCount == 0 || !isInCombo)
            {
                if (_comboCount != 0) _prevInputs.RemoveRange(0, _prevInputs.Count-1);
                _comboCount = 1;
            }
            else
            {
                _comboCount++;
            }

            CurrentAttack = attack;

            if (attack._isSpAtk)
            {
                Vector3 facingDirection = _character._isFacingRight ? transform.right : -transform.right;
                _targetPos = transform.position + facingDirection * 10;
                _animator.runtimeAnimatorController = CurrentAttack._startAnimatorOverride;
                foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(transform.position,facingDirection
                             , 10))
                {
                    if (!raycastHit2D.collider.CompareTag("HurtBox")) continue;
                    HurtBox hurtBox = raycastHit2D.collider.GetComponent<HurtBox>();
                    int targetId = hurtBox.Owner.PlayerId;
                    if (_playerCharacter.PlayerId == targetId) {continue;}
                    _targetPos = raycastHit2D.transform.position;
                    hurtBox.Owner.SetBlockedFramesCount(60);
                    EffectsManager.StartShockwave(transform.position, 3f), 0.2f
                    break;
                }
            }
            else
            {
                _animator.runtimeAnimatorController = attack._animatorOverride;
            }
            _animator.SetTrigger(Attack);
            _animator.SetBool(IsAttacking, true);
        }

        public void StartSpAtk()
        {
            EffectsManager.StartShockwave(_targetPos, 1f);

            Instantiate(CurrentAttack._spAttackGameObject, _targetPos, Quaternion.identity).GetComponent<Wind_elementalspAttack1>().SetCharacter(_playerCharacter);
            _spriteRenderer.enabled = false;
        }
        
        public void SetEndSpAttackAnimator()
        {
            _spriteRenderer.enabled = true;
            _animator.runtimeAnimatorController = CurrentAttack._endAnimatorOverride;
            _animator.SetTrigger(Attack);
            EffectsManager.StartShockwave(_targetPos, 2f, 0.2f);
        }

        private void OnPreUpdate()
        {
            if (_comboCount !=0 && !_isAttacking && _canAttack) _lastAttackFrameAge++;
            if (_comboCount !=0 && _lastAttackFrameAge > _maxLastAttackFrameAge && _canAttack)
            {
                _comboCount = 0;
                _lastAttackFrameAge = 0;
                _prevInputs.Clear();
            }
            
            
            if (_numberOfJumps == _baseNumberOfJumps)
            {
                _numberOfJumps = _baseNumberOfJumps - 1;
            }
            _isGrounded = false;
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + _groundCheckOffset, _groundCheckSize, 0f);

            if (colliders.Length == 0) return;
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.CompareTag("Ground"))
                {
                    _isGrounded = true;
                    _numberOfJumps = _baseNumberOfJumps;
                    break;
                }
            }
            _animator.SetBool(Grounded, _isGrounded);
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
                    _animator.SetFloat(YSpeed, _rb.velocity.y);
                } 
            }
            // TODO: Rotation also rotates nameplate, needs fixing (look at PlayerTest 3.prefab)
            transform.rotation = Quaternion.Euler(new Vector3(0, _isFlipped ? 180 : 0, 0));
        }

        public void SetMoveState(bool status)
        {
            CanMove = status;
            _canAttack = status;
        }

        public void StartedAttacking()
        {
            _isAttacking = true;
            _rb.gravityScale = 0f;
            _lastAttackFrameAge = 0;
        }
        
        public void StopAttacking()
        {
            _isAttacking = false;
            _rb.gravityScale = _gravitySave;
            _lastAttackFrameAge = 0;
            _animator.SetBool(IsAttacking, false);
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
            TickManager.PreUpdate += OnPreUpdate;
        }
     
        private void OnDisable()
        {
            TickManager.FrameUpdate -= OnFrameUpdate;
            TickManager.PreUpdate -= OnPreUpdate;
        }

        public void SetCharacter(Character character)
        {
            _character = character;
            _speed = character._speed;
            _jumpForce = character._jumpForce;
            _groundCheckOffset = character._groundCheckOffset;
            _groundCheckSize = character._groundCheckSize;
        }


        public void SetGrounded(bool state)
        {
            if (state) _numberOfJumps = _baseNumberOfJumps;
            _isGrounded = state;
        }
    }
}

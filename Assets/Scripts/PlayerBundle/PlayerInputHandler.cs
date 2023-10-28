using System;
using System.Collections;
using System.Collections.Generic;
using ProjectAres.Core;
using ProjectAres.ScriptableObjects.Scripts;
using ProjectAres.SpAttacks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.PlayerBundle
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Character Stats")]
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private int _baseNumberOfJumps = 2; // Might be set in character, can be passed from PlayerCharacter.cs
        
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private GameObject _nameplate;
        [SerializeField] private PlayerCharacter _playerCharacter;
        private Character _character;
        
        [Header("Combo System")]
        [SerializeField]private int _maxLastAttackFrameAge = 5;
        private int _comboCount;
        private List<ButtonPos> _prevInputs;
        private int _lastAttackFrameAge;
        private bool _canAttack = true;

        #region Private Variables

        private Vector2 _groundCheckOffset;
        private Vector2 _groundCheckSize;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private bool _isAttacking;
        private float _gravitySave;
        private bool _isFlipped;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int YSpeed = Animator.StringToHash("ySpeed");
        private int _numberOfJumps;
        private bool _hasJumped;
        private Vector3 _tempPosMemory;
        private Vector3 _targetPos;
        private Vector2 MoveVector { get; set; }
        private bool _isGrounded;
        private bool CanMove { get; set; }

        #endregion


        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int Attack = Animator.StringToHash("Attack");

        #region Properties
        public AttackSo CurrentAttack { get; private set; }

        public Animator SelfAnimator => _animator;
        public SpriteRenderer Renderer => _spriteRenderer;

        public GameObject Nameplate => _nameplate;
        public bool IsFlipped => _isFlipped;

        #endregion

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
            // EffectsManager.StartShockwave(transform.position, 1f);
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
            
            // Debug.Log($"IsCombo = {isInCombo}");
            
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
            _animator.SetBool(IsAttacking, true);

            if (attack._isSpAtk)
            {
                if (attack._isInstantCast)
                {
                    StartedAttacking();
                    Instantiate(CurrentAttack._spAttackGameObject, _targetPos, Quaternion.identity).GetComponent<ISpecialAtk>().SetCharacter(_playerCharacter);
                    return;
                }

                _animator.runtimeAnimatorController = attack._startAnimatorOverride;
            }
            else
            {
                _animator.runtimeAnimatorController = attack._animatorOverride;
            }
            _animator.SetTrigger(Attack);
        }

        public void StartSpAtk()
        {
            //EffectsManager.StartShockwave(_targetPos, 1f);

            Instantiate(CurrentAttack._spAttackGameObject, _targetPos, Quaternion.identity).GetComponent<ISpecialAtk>().SetCharacter(_playerCharacter);
        }
        
        public void SetEndSpAttack()
        {
            StopAttacking();
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
            switch (CanMove)
            {
                case true when _isAttacking:
                    _rb.velocity = Vector2.zero;
                    _animator.SetFloat(Speed, 0);
                    break;

                case true:
                    _rb.velocity = new Vector2(MoveVector.x, _rb.velocity.y);
                    _animator.SetFloat(Speed, Math.Abs(MoveVector.x));
                    _animator.SetFloat(YSpeed, _rb.velocity.y);
                    break;
            }

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

        public void TeleportTemp(Vector3 position, float time)
        {
            _tempPosMemory = _playerCharacter.transform.position;
            StartCoroutine(SlideTp(position, time));
        }

        IEnumerator SlideTp(Vector3 endPosition, float time)
        {
            float timeToWait = time / 60;
            float t = 0;
            while (t < time)
            {
                _playerCharacter.transform.position = Vector3.Lerp(_tempPosMemory, endPosition, t);
                t += timeToWait;
                Debug.Log($"TempPosMemory = {_tempPosMemory}");
                yield return new WaitForSeconds(timeToWait);
            }
        }

        public void RolBackTeleport()
        {
            StopCoroutine(nameof(SlideTp));
            _playerCharacter.transform.position = _tempPosMemory;
        }
    
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

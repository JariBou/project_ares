using System;
using NaughtyAttributes;
using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] protected HurtBoxesManager _hurtBoxesManager;
        [SerializeField] protected HitBox _hitBox;
        [SerializeField] protected TMP_Text _text;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected PlayerManager _playerManager;
        [SerializeField, Expandable] protected Character _character;
        
        protected Rigidbody2D _rb;
        protected int IFramesCount;
        protected int BlockedFramesCount;
        protected Vector3 StartPos;
        public int PlayerId { get; protected set; }
        public float PlayerHealth { get; protected set; }
        public Animator Animator => _animator;
        public Character Character => _character;

        public bool IsInvincible { get; protected set; }

        public PlayerManager Manager => _playerManager;
        public HurtBoxesManager HurtBoxManager => _hurtBoxesManager;
        public HitBox HitBoxScript => _hitBox;

        // TODO: maybe make red damage taken indicator duration proportional to current iFrames 
        
        public Damageable WithCharacter(int playerId)
        {
            PlayerId = playerId;
            _character = PlayerManager.Instance.GetCharacterOfPlayer(playerId);
            PlayerHealth = _character._maxHealth;
            _character.Initialize();
            return this;
        }
        
        public float GetPlayerPercentRemainingHp()
        {
            return PlayerHealth / _character._maxHealth;
        }
        
        public Damageable SetId(int playerId)
        {
            PlayerId = playerId;
            return this;
        }
        
        public Damageable SetCharacter(Character character)
        {
            _character = character;
            return this;
        }
        
        public void SetIFrames(int frameCount)
        {
            IFramesCount = frameCount;
            IsInvincible = IFramesCount > 0;
        }
        
        public void ApplyKb(Vector2 force)
        {
            // Debug.Log($"Force Before Compensation: {force}");
            // force.y *= SpeedCompensationFunction(-_rb.velocity.y);
            // Debug.Log($"Force After Compensation: {force}");

            force *= _character._kockbackMultiplier;
            
            _rb.velocity = force;
            // _rb.AddForce(force, ForceMode2D.Impulse);
        }
        
        private float SpeedCompensationFunction(float x)
        {
            return 0.15f * x + 1;
            // return (float)(0.4f * (Math.Exp(0.3f * x) - 1) + 1);
        }
        
        private void OnEnable()
        {
            TickManager.PostUpdate += OnPostUpdate;
        }
        
        private void OnDisable()
        {
            TickManager.PostUpdate -= OnPostUpdate;
        }
        
        protected virtual void OnPostUpdate()
        {
            IsInvincible = IFramesCount > 0;
            IFramesCount = IFramesCount > 0 ? IFramesCount-1 : 0;
            BlockedFramesCount = BlockedFramesCount > 0 ? BlockedFramesCount-1 : 0;
        }
        public virtual void IsAttacked(float damage){}
        
        public virtual void SetBlockedFramesCount(int frameCount)
        {
            BlockedFramesCount = frameCount;
        }

        public void Respawn()
        {
            _rb.velocity = Vector2.zero;
            transform.position = StartPos;
        }

    }
}

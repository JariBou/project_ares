using NaughtyAttributes;
using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.PlayerBundle;
using ProjectAres.ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace ProjectAres
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] protected HurtBoxesManager _hurtBoxesManager;

        [SerializeField] protected TMP_Text _text;
        [SerializeField, Expandable] protected Character _character;
        [SerializeField] protected Animator _animator;
        protected Rigidbody2D _rb;
        public int PlayerId { get; protected set; }
        public float PlayerHealth { get; protected set; }
        public Animator Animator => _animator;
        public Character Character => _character;

        protected int IFramesCount;
        protected int BlockedFramesCount;
        public bool IsInvincible { get; protected set; }

        private BoxCollider2D _pushBox;

        // TODO: maybe make red damage taken indicator duration proportional to current iFrames 
        
        public Damageable WithCharacter(int playerId)
        {
            PlayerId = playerId;
            _character = PlayerManager.Instance.GetCharacterOfPlayer(playerId);
            PlayerHealth = _character._maxHealth;
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
            TickManager.PreUpdate += OnPreUpdate;
        }

        private void OnDisable()
        {
            TickManager.PreUpdate -= OnPreUpdate;
        }

        protected virtual void OnPreUpdate()
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

    }
}

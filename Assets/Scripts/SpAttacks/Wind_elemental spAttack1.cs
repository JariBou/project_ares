using System;
using System.Collections.Generic;
using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.PlayerBundle;
using UnityEngine;

namespace ProjectAres.SpAttacks
{
    public class WindElementalSpAttack1 : MonoBehaviour, ISpecialAtk
    {
        [SerializeField] private List<SpAtkData> _spAtkData;
        [SerializeField] private Color _castColor;
        [SerializeField] private Vector2 _castOffset;
        [SerializeField] private Vector2 _castSize;
        private PlayerCharacter _owner;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private PlayerInputHandler PlayerInputHandler => _owner.InputHandler;
        private Vector3 _targetPos;
        private Vector3 Position => PlayerInputHandler.transform.position;

        private void Start()
        {
            PlayerInputHandler.Nameplate.SetActive(false);
            PlayerInputHandler.Renderer.enabled = false;
            Vector3 facingDirection = PlayerInputHandler.transform.right * (_owner.Character._isFacingRight ? 1 : -1);
            _targetPos = Position + facingDirection * 10;
            PlayerInputHandler.SelfAnimator.runtimeAnimatorController = PlayerInputHandler.CurrentAttack._startAnimatorOverride;
            foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(Position,facingDirection
                         , 10))
            {
                if (!raycastHit2D.collider.CompareTag("HurtBox")) continue;
                HurtBox hurtBox = raycastHit2D.collider.GetComponent<HurtBox>();
                int targetId = hurtBox.Owner.PlayerId;
                if (_owner.PlayerId == targetId) {continue;}
                _targetPos = raycastHit2D.transform.position;
                // TODO: this fucks up everything (enemy gets rooted although he moved)
                hurtBox.Owner.SetBlockedFramesCount(60);
                EffectsManager.StartShockwave(Position, 3f, 0.2f);
                break;
            }

            transform.position = _targetPos;
            PlayerInputHandler.TeleportTemp(_targetPos, 1f);
        }

        public void SetCharacter(PlayerCharacter character)
        {
            _owner = character;
        }

        public void DealDamage(int atkPhase)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + _castOffset, _castSize, 0f);

            foreach (Collider2D col in colliders)
            {
                if (!col.gameObject.CompareTag("HurtBox")) continue;
                HurtBox hurtBox = col.GetComponent<HurtBox>();
            
                int targetId = hurtBox.Owner.PlayerId;
                if (_owner.PlayerId == targetId) {continue;}
                // hurtBox.Owner.transform.position is usually the center of gravity of a player
                Vector2 direction = (hurtBox.Owner.transform.position - transform.position).normalized;
                SpAtkData attackSo = _spAtkData[atkPhase];
                AttackStats attackStats = new(attackSo.Damage, attackSo.KbAmount, direction, attackSo.InvincibilityFramesCount, attackSo.BlockMoveFramesCount);
                PlayerManager.Instance.GameActionsManager.AddPreUpdateAction(new PlayerGameAction(_owner.PlayerId, targetId, PlayerActionType.Attack, attackStats));
            }
        }

        public void EndSpAtk()
        {
            PlayerInputHandler.RolBackTeleport();
            PlayerInputHandler.Nameplate.SetActive(true);
            PlayerInputHandler.Renderer.enabled = true;
            if (PlayerInputHandler.CurrentAttack._hasEndAnimationCast)
            {
                PlayerInputHandler.SelfAnimator.runtimeAnimatorController = PlayerInputHandler.CurrentAttack._endAnimatorOverride;
                PlayerInputHandler.SelfAnimator.SetTrigger(Attack);
            }
            else
            {
                PlayerInputHandler.SetEndSpAttack();
            }
            EffectsManager.StartShockwave(_targetPos, 2f, 0.2f);
            Destroy(gameObject);
        }
        
        
        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {

            Gizmos.color = _castColor;
            Gizmos.DrawWireCube((Vector2)transform.position + _castOffset, _castSize);
            
        }
        #endif
    }

    [Serializable]
    public class SpAtkData
    {
        [SerializeField] private int _damage;
        [SerializeField] private int _kbAmount;
        [SerializeField] private int _invincibilityFramesCount;
        [SerializeField] private int _blockMoveFramesCount;

        public int Damage => _damage;

        public int KbAmount => _kbAmount;

        public int InvincibilityFramesCount => _invincibilityFramesCount;

        public int BlockMoveFramesCount => _blockMoveFramesCount;
    }
}

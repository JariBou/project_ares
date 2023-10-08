using System;
using System.Collections;
using System.Collections.Generic;
using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.PlayerBundle;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;

namespace ProjectAres
{
    public class Wind_elementalspAttack1 : MonoBehaviour
    {
        [SerializeField] private List<SpAtkData> _spAtkData;
        [SerializeField] private Color _castColor;
        [SerializeField] private Vector2 _castOffset;
        [SerializeField] private Vector2 _castSize;
        private PlayerCharacter _owner;

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
            _owner.InputHandler.SetEndSpAttackAnimator();
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

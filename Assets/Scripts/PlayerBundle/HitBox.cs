using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _owner;
        public PlayerCharacter Owner => _owner;

        [SerializeField] private PlayerInputHandler _playerInputHandler;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Collided with {other.gameObject.name}");
            if (!other.gameObject.CompareTag("HurtBox")) return;
            HurtBox hurtBox = other.GetComponent<HurtBox>();
            
            int targetId = hurtBox.Owner.PlayerId;
            if (_owner.PlayerId == targetId) {return;}
            
            // hurtBox.Owner.transform.position is usually the center of gravity of a player
            Vector2 direction = (hurtBox.Owner.transform.position - Owner.transform.position).normalized;
            Debug.Log($"Direction = {direction}");
            AttackSo attackSo = _playerInputHandler.CurrentAttack;
            AttackStats attackStats = new(attackSo._damage, attackSo._kbAmount, direction, attackSo._invincibilityFramesCount, attackSo._blockMoveFramesCount);
            PlayerManager.Instance.GameActionsManager.AddPreUpdateAction(new PlayerGameAction(Owner.PlayerId, targetId, PlayerActionType.Attack, attackStats));
        }
    }
}

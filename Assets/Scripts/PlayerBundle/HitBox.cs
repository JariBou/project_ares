using System;
using Core;
using ProjectAres.Managers;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _owner;
        public PlayerCharacter Owner => _owner;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Collided with {other.gameObject.name}");
            if (other.gameObject.CompareTag("HurtBox"))
            {
                int targetId = other.GetComponent<HurtBox>().Owner.PlayerId;
                PlayerManager.Instance.GameActionsManager.AddPreUpdateAction(new PlayerGameAction(Owner.PlayerId, targetId, PlayerActionType.Attack));
            }
        }
    }
}

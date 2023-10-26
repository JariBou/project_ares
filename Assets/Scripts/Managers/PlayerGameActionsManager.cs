using System.Collections.Generic;
using System.Linq;
using ProjectAres.Core;
using UnityEngine;

namespace ProjectAres.Managers
{
    public class PlayerGameActionsManager : MonoBehaviour
    {
        
        private List<PlayerGameAction> _preUpdateActions = new(16);
        private List<PlayerGameAction> _onUpdateActions = new(16);
        private List<PlayerGameAction> _postUpdateActions = new(16);
        
        public void AddPreUpdateAction(PlayerGameAction action) => _preUpdateActions.Add(action);
        public void AddOnUpdateAction(PlayerGameAction action) => _onUpdateActions.Add(action);
        public void AddPostUpdateAction(PlayerGameAction action) => _postUpdateActions.Add(action);
       

        private void OnPreUpdate()
        {
            IOrderedEnumerable<PlayerGameAction> playerGameActions = _preUpdateActions.OrderBy(action => action.ActionType);

            foreach ((PlayerGameAction action, int index) in playerGameActions.Select((value, i) => ( value, i )))
            {
                // Idk if block should actually be here... maybe a bool on PlayerCharacter?
                // Because rn this implies sending an event to _preUpdateActions every frame to say the player is blocking
                Damageable source = PlayerManager.GetPlayerCharacterStatic(action.OwnerId);
                Damageable target = PlayerManager.GetPlayerCharacterStatic(action.TargetId);
                
                switch (action.ActionType)
                {
                    case PlayerActionType.Attack:
                        if (target.IsInvincible) break;
                        target.Animator.SetTrigger("Hurt");
                        target.IsAttacked(action.AttackStats.Damage);
                        target.SetIFrames(action.AttackStats.InvincibilityFrames);
                        target.SetBlockedFramesCount(action.AttackStats.MoveBlockFrames);
                        target.ApplyKb(action.AttackStats.ForceDirection * action.AttackStats.KbValue);
                        Debug.Log($"Attacked {target.name}");
                        break;
                    case PlayerActionType.Block:
                        
                        break;
                }
            }

            _preUpdateActions = new List<PlayerGameAction>(16);
        }

        private void OnFrameUpdate()
        {
            IOrderedEnumerable<PlayerGameAction> playerGameActions = _onUpdateActions.OrderBy(action => action.ActionType);

            foreach ((PlayerGameAction action, int index) in playerGameActions.Select((value, i) => ( value, i )))
            {
                // Idk if block should actually be here... maybe a bool on PlayerCharacter?
                // Because rn this implies sending an event to _preUpdateActions every frame to say the player is blocking
                Damageable source = PlayerManager.GetPlayerCharacterStatic(action.OwnerId);
                Damageable target = PlayerManager.GetPlayerCharacterStatic(action.TargetId);
                
                switch (action.ActionType)
                {
                    case PlayerActionType.Attack:
                        target.SetIFrames(action.AttackStats.InvincibilityFrames);
                        target.SetBlockedFramesCount(action.AttackStats.MoveBlockFrames);
                        target.ApplyKb(action.AttackStats.ForceDirection * action.AttackStats.KbValue);
                        Debug.Log($"Attacked {target.name}");
                        break;
                    case PlayerActionType.Block:
                        
                        break;
                }
            }

            _onUpdateActions = new List<PlayerGameAction>(16);
        }
        
        private void OnPostUpdate()
        {
            foreach (PlayerGameAction action in _postUpdateActions)
            {
                
            }
            
            _postUpdateActions = new List<PlayerGameAction>(16);
        }
        
        private void OnEnable()
        {
            TickManager.PreUpdate += OnPreUpdate;
            TickManager.FrameUpdate += OnFrameUpdate;
            TickManager.PostUpdate += OnPostUpdate;
        }
        
        private void OnDisable()
        {
            TickManager.PreUpdate -= OnPreUpdate;
            TickManager.FrameUpdate -= OnFrameUpdate;
            TickManager.PostUpdate -= OnPostUpdate;
        }
    }
}

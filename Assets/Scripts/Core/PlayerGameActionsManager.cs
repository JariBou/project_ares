using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
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
            foreach (PlayerGameAction action in _preUpdateActions)
            {
                
            }

            _preUpdateActions = new List<PlayerGameAction>(16);
        }

        private void OnFrameUpdate()
        {
            foreach (PlayerGameAction action in _onUpdateActions)
            {
                
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

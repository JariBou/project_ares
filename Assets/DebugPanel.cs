using System;
using System.Collections;
using System.Collections.Generic;
using GraphicsLabor.Scripts;
using GraphicsLabor.Scripts.Core;
// using GraphicsLabor.GraphicsLabor.Scripts;
using NaughtyAttributes;
using ProjectAres.Managers;
using ProjectAres.PlayerBundle;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace ProjectAres
{
    public class DebugPanel : MonoBehaviour
    {
        public PlayerGizmoEnum PlayerGizmoDisplay { get; private set; }
        private PlayerManager _playerManager;


        private void Start()
        {
            PlayerGizmoDisplay = PlayerGizmoEnum.None;
            _playerManager = GameObject.FindWithTag("Managers").GetComponent<PlayerManager>();
        }

        private void DrawPlayersHurtBoxes()
        {
            if (PlayerGizmoDisplay is not (PlayerGizmoEnum.HurtBoxes or PlayerGizmoEnum.Both)) return;
            
            foreach (Damageable damageable in _playerManager.GetPlayers())
            {
                PlayerCharacter player = (PlayerCharacter)damageable;
                foreach (HurtBox hurtBox in player.HurtBoxManager.HurtBoxes)
                {
                    if (hurtBox.Collider is BoxCollider2D hurtBoxCollider)
                    {
                        Vector2 flipMultiplier = new((player.InputHandler.IsFlipped ? -1 : 1), 1);
                        Drawer2D.DrawWiredQuad((Vector2)player.transform.position + hurtBoxCollider.offset * flipMultiplier
                            ,
                            hurtBoxCollider.size, Color.blue);
                    }
                }
            }
        }
        
        private void DrawPlayersHitBoxes()
        {
            if (PlayerGizmoDisplay is not (PlayerGizmoEnum.HitBoxes or PlayerGizmoEnum.Both)) return;
            
            foreach (Damageable damageable in _playerManager.GetPlayers())
            {
                PlayerCharacter player = (PlayerCharacter)damageable;
                if (player.HitBoxScript.Collider is BoxCollider2D { enabled: true } hitBoxCollider)
                {
                    Vector2 flipMultiplier = new((player.InputHandler.IsFlipped ? -1 : 1), 1);
                    Drawer2D.DrawWiredQuad((Vector2)player.transform.position + hitBoxCollider.offset * flipMultiplier,
                        hitBoxCollider.size, Color.red);
                }
            }
        }

        private void OnEnable()
        {
            Drawer2D.DrawCallback += DrawPlayersHurtBoxes;
            Drawer2D.DrawCallback += DrawPlayersHitBoxes;
        }
        
        private void OnDisable()
        {
            Drawer2D.DrawCallback -= DrawPlayersHurtBoxes;
            Drawer2D.DrawCallback -= DrawPlayersHitBoxes;
        }

        public void OnPlayerGizmoDropdownChange(int index)
        {
            PlayerGizmoDisplay = (PlayerGizmoEnum)index;
        }
    }

    public enum PlayerGizmoEnum
    {
        None,
        HitBoxes,
        HurtBoxes,
        Both
    }
}

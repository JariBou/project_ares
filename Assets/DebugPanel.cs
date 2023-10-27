using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ProjectAres.Managers;
using ProjectAres.PlayerBundle;
using UnityEditor;
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

        // TODO: FUCK THIS SHIT DOESNT WORK
        private void OnDrawGizmos()
        {
            if (PlayerGizmoDisplay == PlayerGizmoEnum.None) return;
            
            foreach (Damageable player in _playerManager.GetPlayers())
            {
                if (PlayerGizmoDisplay is PlayerGizmoEnum.HurtBoxes or PlayerGizmoEnum.Both)
                {
                    foreach (HurtBox hurtBox in player.HurtBoxManager.HurtBoxes)
                    {
                        if (hurtBox.Collider is BoxCollider2D hurtBoxCollider)
                        {
                            Gizmos.DrawWireCube(player.transform.position + (Vector3)hurtBoxCollider.offset,
                                hurtBoxCollider.size);
                        }
                    }
                }

                if (PlayerGizmoDisplay is PlayerGizmoEnum.HitBoxes or PlayerGizmoEnum.Both)
                {
                    if (player.HitBoxScript.Collider is BoxCollider2D { enabled: true } hitBoxCollider)
                    {
                        Gizmos.DrawWireCube(player.transform.position + (Vector3)hitBoxCollider.offset,
                            hitBoxCollider.size);
                    }
                }
            }
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

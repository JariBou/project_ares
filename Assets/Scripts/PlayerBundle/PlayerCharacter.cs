using System;
using Core;
using ProjectAres.Managers;
using ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private HurtBoxesManager _hurtBoxesManager;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private Character _character;
        [SerializeField] private Animator _animator;
        public int PlayerId { get; private set; }
        public Animator Animator => _animator;

        // Start is called before the first frame update
        void Start()
        {
            _text.text = _character._name;
            _playerInputHandler.SetCharacterStats(_character);
            _hurtBoxesManager.SetOwners(this);
        }

        public PlayerCharacter WithCharacter(int playerId)
        {
            PlayerId = playerId;
            _character = PlayerManager.Instance.GetCharacterOfPlayer(playerId);
            return this;
        }
        
    }
}

using System;
using ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace PlayerBundle
{
    public class PlayerCharacterInfo : MonoBehaviour
    {
        private PlayerInputHandler _playerInputHandler;

        [SerializeField] private TMP_Text _text;
        public Character _character;

        private void Awake()
        {
            _playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _text.text = _character._name;
            _playerInputHandler.SetCharacterStats(_character);
        }
        
    }
}

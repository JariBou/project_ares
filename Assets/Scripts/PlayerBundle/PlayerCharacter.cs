using ProjectAres.ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _playerInputHandler;

        [SerializeField] private TMP_Text _text;
        public Character _character;
        
        // Start is called before the first frame update
        void Start()
        {
            _text.text = _character._name;
            _playerInputHandler.SetCharacterStats(_character);
        }
        
    }
}

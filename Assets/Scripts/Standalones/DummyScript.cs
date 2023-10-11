using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;

namespace ProjectAres.Standalones
{
    public class DummyScript : Damageable
    {
        [SerializeField] private Character _dummyCharacter;
        private PlayerManager _playerManager;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerManager = GameObject.FindWithTag("Managers").GetComponent<PlayerManager>();
        }
        
        private void Start()
        {
            int id = _playerManager.AddDummy(this);
            _text.text = _character._name;
            _hurtBoxesManager.SetOwners(this);
            SetId(id);
            SetCharacter(_dummyCharacter);
        }
        
    }
}

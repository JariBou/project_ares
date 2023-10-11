using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;

namespace ProjectAres.Standalones
{
    public class DummyScript : Damageable
    {
        [SerializeField] private Character _dummyCharacter;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            PlayerManager.Instance.PlayerCharacters.Add(this);
            int id = PlayerManager.Instance.PlayerCharacters.IndexOf(this);
            _text.text = _character._name;
            _hurtBoxesManager.SetOwners(this);
            SetId(id);
            SetCharacter(_dummyCharacter);
        }
        
    }
}

using ProjectAres.Managers;
using ProjectAres.PlayerBundle;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;

namespace ProjectAres.Standalones
{
    public class DummyScript : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _selfPlayerCharacter;
        [SerializeField] private Character _dummyCharacter;
        
        private void Start()
        {
            PlayerManager.Instance.PlayerCharacters.Add(_selfPlayerCharacter);
            int id = PlayerManager.Instance.PlayerCharacters.IndexOf(_selfPlayerCharacter);
            _selfPlayerCharacter.SetId(id).SetCharacter(_dummyCharacter);
        }
        
    }
}

using ProjectAres.Managers;
using UnityEngine;

namespace ProjectAres
{
    public class PlayerInfoDisplay : MonoBehaviour
    {
        private PlayerManager _playerManager;
        [SerializeField] private GameObject _displayPrefab;
        private void Awake()
        {
            _playerManager = GameObject.FindWithTag("Managers").GetComponent<PlayerManager>();
        }

        private void Start()
        {

            foreach (Damageable playerCharacter in _playerManager.PlayerCharacters)
            {
                if (_playerManager.DummiesIdList.Contains(playerCharacter.PlayerId)) continue;
                Instantiate(_displayPrefab, transform).GetComponent<CharacterInfoDisplay>().SetPlayerCharacter(playerCharacter);
            }
        }
    }
}

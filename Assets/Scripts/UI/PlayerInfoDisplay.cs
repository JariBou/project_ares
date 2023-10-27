using ProjectAres.Managers;
using ProjectAres.PlayerBundle;
using UnityEngine;

namespace ProjectAres.UI
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

            foreach (Damageable playerCharacter in _playerManager.GetPlayers())
            {
                Instantiate(_displayPrefab, transform).GetComponent<CharacterInfoDisplay>().SetPlayerCharacter(playerCharacter);
            }
        }
    }
}

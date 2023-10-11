using System.Collections.Generic;
using System.Linq;
using ProjectAres.Core;
using ProjectAres.PlayerBundle;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectAres.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private List<PlayerConfiguration> _playerConfigs;
    
        [SerializeField]
        private int _maxPlayers = 2;
        [SerializeField]
        private int _numberOfDummies = 1;

        public int MaxPlayers => _maxPlayers;

        [SerializeField] private ScenePlayerSpawnInfo _scenePlayerSpawnInfos;

        public static PlayerManager Instance { get; private set; }
        public List<Damageable> PlayerCharacters { get; private set; }

        public List<PlayerConfiguration> PlayerConfigs => _playerConfigs;

        private PlayerInputManager _inputManager;

        [SerializeField] private CharacterManager _characterManager;
        public CharacterManager CharacterManager => _characterManager;
        [SerializeField] private PlayerGameActionsManager _gameActionsManager;
        public PlayerGameActionsManager GameActionsManager => _gameActionsManager;

        public List<int> DummiesIdList { get; private set; }

        private string _testString = "";

        private void Awake()
        {
            if(Instance != null)
            {
                Debug.Log("[Singleton] Trying to instantiate a second instance of a singleton class.");
            }
            else
            {
                Instance = this;
                _playerConfigs = new List<PlayerConfiguration>(_maxPlayers); // Doesn't need dummy player in this, reserved ol' regular players
                PlayerCharacters = new List<Damageable>(_maxPlayers+_numberOfDummies); // for dummy players
                _inputManager = GetComponent<PlayerInputManager>();
                DummiesIdList = new List<int>(_numberOfDummies);
            }
        }

        public int AddDummy(Damageable dummy)
        {
            PlayerCharacters.Add(dummy);
            int id = PlayerCharacters.IndexOf(dummy);
            DummiesIdList.Add(id);
            return id;
        }

        private void Start()
        {
            if (_scenePlayerSpawnInfos == null)
            {
                _scenePlayerSpawnInfos = GameObject.FindWithTag("SceneInfoProvider").GetComponent<ScenePlayerSpawnInfo>();
            }

            _inputManager.joinBehavior = _scenePlayerSpawnInfos._joinBehavior;
            _inputManager.playerPrefab = _scenePlayerSpawnInfos._playerPrefab;
        }

        public void HandlePlayerJoin(PlayerInput pi)
        {
            int playerIndex = pi.playerIndex;
            string debugString = "";
            foreach (InputDevice inputDevice in pi.devices)
            {
                if (inputDevice is UnityEngine.InputSystem.XInput.XInputController) // I cannot stress how much I HATE this
                {
                    Destroy(pi.gameObject);
                    return;
                }
                debugString += inputDevice.name + ", ";
            }
            Debug.Log($"Player {playerIndex} joined with inputs {debugString}   " );
            Debug.Log($"TestString: {_testString}");
            
            if(PlayerConfigs.All(p => p.PlayerIndex != playerIndex))
            {
                pi.GetComponent<PlayerSelection>().SetPlayerId(playerIndex);
                PlayerConfigs.Add(new PlayerConfiguration(pi));
            }

            Transform parent =
                _scenePlayerSpawnInfos._playerSpawnPoints[
                    playerIndex % _scenePlayerSpawnInfos._playerSpawnPoints.Count];
             

            pi.transform.SetParent(parent);
            if (_scenePlayerSpawnInfos._alsoSetPosition)
            {
                pi.transform.position = parent.position;
            }

            switch (_scenePlayerSpawnInfos._sceneBuildIndex)
            {
                case SceneBuildIndex.Scene:
                    PlayerCharacters.Add(pi.GetComponent<PlayerCharacter>().WithCharacter(playerIndex));
                    break;
                case SceneBuildIndex.SelectionMenu:
                    //pi.GetComponent<PlayerSelection>().SetActive();
                    break;
            }

            PlayerConfigs[playerIndex].InputDevices = pi.devices.ToArray();
        }

        // Player Id = PlayerIndex
        public Character GetCharacterOfPlayer(int playerId)
        {
            return _characterManager[PlayerConfigs[playerId].SelectionIndex];
        }
        
        public Damageable GetPlayerCharacter(int playerId)
        {
            return PlayerCharacters[playerId];
        }
        
        public static Character GetCharacterOfPlayerStatic(int playerId)
        {
            return Instance._characterManager[Instance.PlayerConfigs[playerId].SelectionIndex];
        }
        
        public static Damageable GetPlayerCharacterStatic(int playerId)
        {
            return Instance.PlayerCharacters[playerId];
        }

        public void ReadyPlayer(int index)
        {
            PlayerConfigs[index].IsReady = true;
            if (PlayerConfigs.All(p => p.IsReady))
            {
                SceneManager.LoadScene((int) SceneBuildIndex.Scene);
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _scenePlayerSpawnInfos = GameObject.FindWithTag("SceneInfoProvider").GetComponent<ScenePlayerSpawnInfo>();
            _inputManager.joinBehavior = _scenePlayerSpawnInfos._joinBehavior;
            _inputManager.playerPrefab = _scenePlayerSpawnInfos._playerPrefab;

            if (_scenePlayerSpawnInfos._joinBehavior == PlayerJoinBehavior.JoinPlayersManually)
            {
                SpawnPlayers();
            }
        }

        private void SpawnPlayers()
        {
            foreach (PlayerConfiguration playerConfig in PlayerConfigs)
            {
                _testString = $"Spawning player with id {playerConfig.PlayerIndex}";
                // TODO: Do this lmao
                _inputManager.playerPrefab = _characterManager[playerConfig.SelectionIndex]._characterPrefab;
                playerConfig.ChangeInput(_inputManager.JoinPlayer(playerIndex: playerConfig.PlayerIndex, pairWithDevices: playerConfig.InputDevices));
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    

    
}
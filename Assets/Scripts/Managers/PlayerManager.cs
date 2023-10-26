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
        // TODO: Remove Instance
        public static PlayerManager Instance { get; private set; }
    
        [SerializeField] private CharacterManager _characterManager;
        [Header("Game Settings")]
        [SerializeField] private int _maxPlayers = 2;
        [SerializeField] private int _numberOfDummies = 1;
        [SerializeField] private bool _displayCharacterName = true;
        [SerializeField] private List<Color> _playerColors;

        #region Private Fields
        
        private PlayerGameActionsManager _gameActionsManager;
        private ScenePlayerSpawnInfo _scenePlayerSpawnInfos;
        private PlayerInputManager _inputManager;
        private List<PlayerConfiguration> _playerConfigs;
        
        #endregion

        #region Properties

        public List<Damageable> PlayerCharacters { get; private set; }
        public List<PlayerConfiguration> PlayerConfigs => _playerConfigs;
        public CharacterManager CharacterManager => _characterManager;
        public PlayerGameActionsManager GameActionsManager => _gameActionsManager;
        public List<int> DummiesIdList { get; private set; }
        public bool DisplayCharacterName => _displayCharacterName;

        #endregion
        
        
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
                _gameActionsManager = GetComponent<PlayerGameActionsManager>();
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
                default:
                    Debug.LogWarning("Scene not handled by Player Manager");
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
            _inputManager.playerPrefab = _scenePlayerSpawnInfos._playerPrefab;
            _inputManager.joinBehavior = _scenePlayerSpawnInfos._joinBehavior;

            if (_scenePlayerSpawnInfos._joinBehavior == PlayerJoinBehavior.JoinPlayersManually)
            {
                SpawnPlayers();
            }
        }

        private void SpawnPlayers()
        {
            foreach (PlayerConfiguration playerConfig in PlayerConfigs)
            {
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

        public Color GetColorOfPlayer(int playerId)
        {
            if (playerId >= _playerColors.Count)
            {
                Debug.LogError("Player ID out of range of _PlayerColors");
                return Color.black;
            }
            return _playerColors[playerId];
        }
    }
}
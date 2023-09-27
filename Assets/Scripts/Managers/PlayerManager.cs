using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using ProjectAres.PlayerBundle;
using ProjectAres.ScriptableObjects.Scripts;
using ProjectAres.Standalones;
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

        [SerializeField] private ScenePlayerSpawnInfo _scenePlayerSpawnInfos;

        public static PlayerManager Instance { get; private set; }

        public List<PlayerConfiguration> PlayerConfigs => _playerConfigs;
        public CharacterManager CharacterManager => _characterManager;

        private PlayerInputManager _inputManager;

        [SerializeField] private CharacterManager _characterManager;

        private void Awake()
        {
            if(Instance != null)
            {
                Debug.Log("[Singleton] Trying to instantiate a second instance of a singleton class.");
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                _playerConfigs = new List<PlayerConfiguration>();
                _inputManager = GetComponent<PlayerInputManager>();
            }
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
            Debug.Log("player joined " + playerIndex);

            if(PlayerConfigs.All(p => p.PlayerIndex != playerIndex))
            {
                pi.GetComponent<PlayerSelection>().SetPlayerId(playerIndex);
                PlayerConfigs.Add(new PlayerConfiguration(pi));
            }

            Transform parent = _scenePlayerSpawnInfos._playerSpawnPoints.Count > 1
                ? _scenePlayerSpawnInfos._playerSpawnPoints[playerIndex % _scenePlayerSpawnInfos._playerSpawnPoints.Count]
                : _scenePlayerSpawnInfos._playerSpawnPoints[0];

            pi.transform.SetParent(parent);
            if (_scenePlayerSpawnInfos._alsoSetPosition)
            {
                pi.transform.position = parent.position;
            }

            switch (_scenePlayerSpawnInfos._sceneBuildIndex)
            {
                case SceneBuildIndex.Scene:
                    pi.GetComponent<PlayerCharacter>()._character = _characterManager[PlayerConfigs[playerIndex].SelectionIndex];
                    break;
                case SceneBuildIndex.SelectionMenu:
                    //pi.GetComponent<PlayerSelection>().SetActive();
                    break;
            }

            PlayerConfigs[playerIndex].InputDevices = pi.devices.ToArray();
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
            for (int i = 0; i < PlayerConfigs.Count; i++)
            {
                PlayerConfigs[i].ChangeInput(_inputManager.JoinPlayer(playerIndex: PlayerConfigs[i].PlayerIndex, pairWithDevices: PlayerConfigs[i].InputDevices));
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
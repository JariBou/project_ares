using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Scripts;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerConfiguration> _playerConfigs;
    
    [SerializeField]
    private int _maxPlayers = 2;

    [SerializeField] private ScenePlayerSpawnInfo ScenePlayerSpawnInfos;

    public static PlayerManager Instance { get; private set; }

    private PlayerInputManager _inputManager;

    [SerializeField] private ColorSelection _colorSelection;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a seccond instance of a singleton class.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            _playerConfigs = new List<PlayerConfiguration>();
        }

        _inputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        if (ScenePlayerSpawnInfos == null)
        {
            ScenePlayerSpawnInfos = GameObject.FindWithTag("SceneInfoProvider").GetComponent<ScenePlayerSpawnInfo>();
        }

        _inputManager.joinBehavior = ScenePlayerSpawnInfos._joinBehavior;
        _inputManager.playerPrefab = ScenePlayerSpawnInfos._playerPrefab;
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("player joined " + pi.playerIndex);

        if(_playerConfigs.All(p => p.PlayerIndex != pi.playerIndex))
        {
            pi.GetComponent<PlayerSelection>().SetPlayerId(pi.playerIndex);
            _playerConfigs.Add(new PlayerConfiguration(pi));
        }

        Transform parent = ScenePlayerSpawnInfos._playerSpawnPoints.Count > 1
            ? ScenePlayerSpawnInfos._playerSpawnPoints[pi.playerIndex % ScenePlayerSpawnInfos._playerSpawnPoints.Count]
            : ScenePlayerSpawnInfos._playerSpawnPoints[0];

        pi.transform.SetParent(parent);
        if (ScenePlayerSpawnInfos._alsoSetPosition)
        {
            pi.transform.position = parent.position;
        }

        pi.GetComponent<SpriteRenderer>().color = _colorSelection._possibleColors[_playerConfigs[pi.playerIndex].SelectionIndex];

    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;
        if (_playerConfigs.All(p => p.IsReady))
        {
            Debug.Log("Yeah");
            SceneManager.LoadScene(1);
        }
    }

   

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ScenePlayerSpawnInfos = GameObject.FindWithTag("SceneInfoProvider").GetComponent<ScenePlayerSpawnInfo>();
        _inputManager.joinBehavior = ScenePlayerSpawnInfos._joinBehavior;
        _inputManager.playerPrefab = ScenePlayerSpawnInfos._playerPrefab;

        if (ScenePlayerSpawnInfos._joinBehavior == PlayerJoinBehavior.JoinPlayersManually)
        {
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < _playerConfigs.Count; i++)
        {
            _playerConfigs[i].ChangeInput(_inputManager.JoinPlayer(playerIndex: i, pairWithDevices: _playerConfigs[i].Input.devices.ToArray()));
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

[Serializable]
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    
    public PlayerConfiguration(PlayerInput pi, int selectionIndex)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
        SelectionIndex = selectionIndex;
    }

    public void ChangeInput(PlayerInput pi)
    {
        Input = pi;
    }

    public PlayerInput Input { get; private set; }
    public int PlayerIndex { get; private set; }
    public bool IsReady { get; set; }
    public int SelectionIndex { get; set; }

}

public enum SceneBuildIndex
{
    SelectionMenu =0,
    Scene = 1,
}

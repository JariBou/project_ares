using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerBundle;
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

    public List<PlayerConfiguration> PlayerConfigs => _playerConfigs;
    public CharacterManager CharacterManager => _characterManager;

    private PlayerInputManager _inputManager;

    [SerializeField] private ColorSelection _colorSelection;
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
        if (ScenePlayerSpawnInfos == null)
        {
            ScenePlayerSpawnInfos = GameObject.FindWithTag("SceneInfoProvider").GetComponent<ScenePlayerSpawnInfo>();
        }

        _inputManager.joinBehavior = ScenePlayerSpawnInfos._joinBehavior;
        _inputManager.playerPrefab = ScenePlayerSpawnInfos._playerPrefab;
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

        Transform parent = ScenePlayerSpawnInfos._playerSpawnPoints.Count > 1
            ? ScenePlayerSpawnInfos._playerSpawnPoints[playerIndex % ScenePlayerSpawnInfos._playerSpawnPoints.Count]
            : ScenePlayerSpawnInfos._playerSpawnPoints[0];

        pi.transform.SetParent(parent);
        if (ScenePlayerSpawnInfos._alsoSetPosition)
        {
            pi.transform.position = parent.position;
        }

        switch (ScenePlayerSpawnInfos._sceneBuildIndex)
        {
            case SceneBuildIndex.Scene:
                pi.GetComponent<SpriteRenderer>().color = _colorSelection._possibleColors[PlayerConfigs[playerIndex].SelectionIndex];
                pi.GetComponent<PlayerCharacterInfo>()._character = _characterManager[PlayerConfigs[playerIndex].SelectionIndex];
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

    // TODO: Colors get inverted for some reason, actually its more of a the plaeyer on the left is on the right and vice versa
    // Ptentially intended behaviour (spawn points are inverted lmao)
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
    public InputDevice[] InputDevices { get; set; }
    
    // Maybe get a reference to the SO of the character here when pressing confirm idk, or just have another static class with all SOs

}

public enum SceneBuildIndex
{
    SelectionMenu =0,
    Scene = 1,
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.Core
{
    [Serializable]
    public class ScenePlayerSpawnInfo : MonoBehaviour
    {
        [Tooltip("The scene this object is in")] public SceneBuildIndex _sceneBuildIndex;
        [Tooltip("How we should handle players joining")] public PlayerJoinBehavior _joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        [Tooltip("The Player Prefab the PlayerManagerShould Spawn if needed")] public GameObject _playerPrefab;
        [Tooltip("Possible SpawnPoints for the players")] public List<Transform> _playerSpawnPoints;
        [Tooltip("Should the position of the player's gameobject be changed when instantiating it")] public bool _alsoSetPosition;
    }
}

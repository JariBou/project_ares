using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.Core
{
    [Serializable]
    public class ScenePlayerSpawnInfo : MonoBehaviour
    {
        public SceneBuildIndex _sceneBuildIndex;
        public PlayerJoinBehavior _joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        public List<Transform> _playerSpawnPoints;
        public GameObject _playerPrefab;
        public bool _alsoSetPosition;
    }
}

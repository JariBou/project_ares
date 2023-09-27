using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.Standalones
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
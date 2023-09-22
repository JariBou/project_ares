using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetCameraScript : MonoBehaviour
{
    private CinemachineTargetGroup _cinemachineTargetGroup;
    [SerializeField] private int _weight = 1;
    [SerializeField] private int _radius = 2;
    
    private void Awake()
    {
        _cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        foreach (PlayerConfiguration playerConfig in PlayerManager.Instance.GetPlayerConfigs())
        {
            _cinemachineTargetGroup.AddMember(playerConfig.Input.transform, _weight, _radius);
        }
    }
}

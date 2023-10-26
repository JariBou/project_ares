using System;
using System.Collections.Generic;
using ProjectAres.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectAres.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AttackCombo")]
    public class AttackComboSo : ScriptableObject
    {
        [FormerlySerializedAs("Attacks")] public List<AttackComboBinding> _attacks;
        public int Count => _attacks.Count;
    }

    [Serializable]
    public class AttackComboBinding
    {
        [SerializeField] private AttackSo _attack;
        [SerializeField] private ButtonPos _buttonPos;

        public AttackComboBinding(AttackSo attack, ButtonPos buttonPos)
        {
            _attack = attack;
            _buttonPos = buttonPos;
        }

        public AttackSo Attack => _attack;

        public ButtonPos ButtonPos => _buttonPos;
    }
}

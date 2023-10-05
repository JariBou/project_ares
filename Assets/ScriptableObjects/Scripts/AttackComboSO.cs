using System;
using System.Collections.Generic;
using ProjectAres.Core;
using UnityEngine;

namespace ProjectAres.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AttackCombo")]
    public class AttackComboSO : ScriptableObject
    {
        public List<AttackComboBinding> Attacks;
        public int Count => Attacks.Count;
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

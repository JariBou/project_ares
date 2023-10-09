using System;
using System.Collections.Generic;
using NaughtyAttributes;
using ProjectAres.Core;
using UnityEngine;

namespace ProjectAres.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Character")]
    public class Character : ScriptableObject
    {
        public string _name = "New Blank Character";
        public CharacterType _characterType = CharacterType.None;

        public int _weight; // Weight is going to be used to calculate Knockback
        public int _speed;
        public int _jumpForce; // Theoretically shouldn't change, only weight should change, nvm only changing weight isn't enough
        // should make some math to find an easy way to setup, maybe with some curves
        public Vector2 _groundCheckOffset;
        public Vector2 _groundCheckSize;

        public List<AttackComboSO> _attackCombos;
        public bool _isFacingRight;
        public GameObject _characterPrefab;
        
        public void ApplyPreset(Character preset)
        {
            _name = preset._name;
            _weight = preset._weight;
            _speed = preset._speed;
            _jumpForce = preset._jumpForce;
            _characterType = preset._characterType;
        }


        public List<AttackComboSO> GetPossibleCombos(List<ButtonPos> previousInputs)
        {
            List<AttackComboSO> possibleCombos = new List<AttackComboSO>(_attackCombos.Count);

            foreach (AttackComboSO comboSo in _attackCombos)
            {
                if (HasCombination(comboSo, previousInputs))
                {
                    possibleCombos.Add(comboSo);
                }
            }

            return possibleCombos;
        }
        /// <summary>
        /// Returns the first Combo found with the provided inputs
        /// </summary>
        /// <param name="previousInputs"></param>
        /// <returns>Current combo SO</returns>
        /// <exception cref="InvalidOperationException">Throws an error if does not find a suitable current combo</exception>
        public AttackComboSO GetCurrentCombo(List<ButtonPos> previousInputs)
        {
            foreach (AttackComboSO comboSo in _attackCombos)
            {
                if (HasCombination(comboSo, previousInputs))
                {
                    return comboSo;
                }
            }
        
            throw new InvalidOperationException($"No Current Combo on Character '{_name}' for inputs '{previousInputs}'");
        }
        
        /// <summary>
        /// Returns the current combo attack defined by previous inputs and comboCount
        /// Defaults to the Base Attack of the last provided Input
        /// </summary>
        /// <param name="previousInputs"></param>
        /// <param name="comboCount"></param>
        /// <returns></returns>
        public AttackSo GetCurrentComboAttack(List<ButtonPos> previousInputs, int comboCount)
        {
            try
            {
                return GetCurrentCombo(previousInputs).Attacks[comboCount].Attack;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return GetBaseAttackOfInput(previousInputs[^1]);
            }
        }
        
        /// <summary>
        /// Assigns the current attackSO to a referenced variable and returns true if in combo and false if out of combo
        /// Defaults to the Base Attack of the last provided Input
        /// </summary>
        /// <param name="previousInputs"></param>
        /// <param name="comboCount"></param>
        /// <param name="attack">The current attack reference</param>
        /// <returns></returns>
        public bool GetCurrentComboAttack(List<ButtonPos> previousInputs, int comboCount, out AttackSo attack)
        {
            try
            {
                attack = GetCurrentCombo(previousInputs).Attacks[comboCount].Attack;
                return true;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                attack = GetBaseAttackOfInput(previousInputs[^1]);
                return false;
            }
        }

        public AttackSo GetBaseAttackOfInput(ButtonPos input)
        {
            foreach (AttackComboSO attackComboSo in _attackCombos)
            {
                if (attackComboSo.Attacks[0].ButtonPos == input)
                {
                    return attackComboSo.Attacks[0].Attack;
                }            
            }

            throw new InvalidOperationException($"No base Attack on Character '{_name}' for input '{input.ToString()}'");
        }

        private bool HasCombination(AttackComboSO combo, List<ButtonPos> comboInputs)
        {
            if (combo.Count < comboInputs.Count) return false;
            for (int i = 0; i < comboInputs.Count; i++)
            {
                if (combo.Attacks[i].ButtonPos != comboInputs[i])
                {
                    return false;
                }
            }

            return true;
        }

    }


    public enum CharacterType
    {
        None =3,
        Light = 0,
        Regular = 1,
        Heavy = 2,
    }
}
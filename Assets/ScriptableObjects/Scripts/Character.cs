using System;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using ProjectAres.Core;
using UnityEditor;
using UnityEngine;

namespace ProjectAres.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Character")]
    public class Character : ScriptableObject
    {

        [Header("Base Stats")]
        public string _name = "New Blank Character";
        public CharacterType _characterType = CharacterType.None;
        public int _maxHealth = 100;
        public int _weight; // Weight is going to be used to calculate Knockback
        public int _speed;
        public int _jumpForce; // Theoretically shouldn't change, only weight should change, nvm only changing weight isn't enough
        // should make some math to find an easy way to setup, maybe with some curves

        [Header("Ground Check")]
        public Vector2 _groundCheckOffset;
        public Vector2 _groundCheckSize;

        [Space]
        public List<AttackComboSo> _attackCombos;
        [Header("Display and Spawning")]
        public GameObject _characterPrefab;
        public Sprite _characterIcon;
        public bool _isFacingRight;

        public void ApplyPreset(Character preset)
        {
            _name = preset._name;
            _weight = preset._weight;
            _speed = preset._speed;
            _jumpForce = preset._jumpForce;
            _characterType = preset._characterType;
        }

        /// <summary>
        /// Returns the first Combo found with the provided inputs
        /// </summary>
        /// <param name="previousInputs"></param>
        /// <returns>Current combo SO</returns>
        /// <exception cref="InvalidOperationException">Throws an error if does not find a suitable current combo</exception>
        public AttackComboSo GetCurrentCombo(List<ButtonPos> previousInputs)
        {
            foreach (AttackComboSo comboSo in _attackCombos)
            {
                if (HasCombination(comboSo, previousInputs))
                {
                    return comboSo;
                }
            }
        
            throw new InvalidOperationException($"No Current Combo on Character '{_name}' for inputs '{previousInputs}'");
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
                attack = GetCurrentCombo(previousInputs)._attacks[comboCount].Attack;
                return true;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                attack = GetBaseAttackOfInput(previousInputs[^1]);
                return false;
            }
        }

        [Button]
        private void GetAllCreatedCombos()
        {
            string pathToFile = AssetDatabase.GetAssetPath(this);
            string[] pathToDirectory = pathToFile.Split("/")[Range.EndAt(^1)];
            string path = "";
            foreach (string folder in pathToDirectory)
            {
                path += folder + "/";
            }
            path += "Combos";
            DirectoryInfo info = new(path);
            FileInfo[] fileInfo = info.GetFiles();
            _attackCombos = new List<AttackComboSo>();
            foreach (FileInfo fileInfo1 in fileInfo)
            {
                string filename = fileInfo1.Name;
                if (filename.EndsWith(".meta")) continue;
                _attackCombos.Add(AssetDatabase.LoadAssetAtPath<AttackComboSo>(path+$"/{filename}"));
            }
        }

        private AttackSo GetBaseAttackOfInput(ButtonPos input)
        {
            foreach (AttackComboSo attackComboSo in _attackCombos)
            {
                if (attackComboSo._attacks[0].ButtonPos == input)
                {
                    return attackComboSo._attacks[0].Attack;
                }            
            }

            throw new InvalidOperationException($"No base Attack on Character '{_name}' for input '{input.ToString()}'");
        }

        private bool HasCombination(AttackComboSo combo, List<ButtonPos> comboInputs)
        {
            if (combo.Count < comboInputs.Count) return false;
            for (int i = 0; i < comboInputs.Count; i++)
            {
                if (combo._attacks[i].ButtonPos != comboInputs[i])
                {
                    return false;
                }
            }

            return true;
        }

    }


}
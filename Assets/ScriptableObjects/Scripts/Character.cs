using System.Collections.Generic;
using Core;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects.Scripts
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

        private bool HasCombination(AttackComboSO combo, List<ButtonPos> comboInputs)
        {
            for (int i = 0; i < comboInputs.Count; i++)
            {
                if (combo.Attacks[i]._buttonPos != comboInputs[i])
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
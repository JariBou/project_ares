using System;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Character")]
    public class Character : ScriptableObject
    {
        public string _name = "Character presetOf(Regular)";
        public CharacterType _characterType = CharacterType.Regular;

        public int _weight = 5;
        public int _speed = 10;
        public int _jumpForce = 20; // Theoretically should'nt change, only weight should change
        
        
        
        public void ApplyPreset(CharacterManager.CharacterPreset preset)
        {
            _name = preset._name;
            _weight = preset._weight;
            _speed = preset._speed;
            _jumpForce = preset._jumpForce;
            _characterType = preset._characterType;
        }

    } 
}


public enum CharacterType
{
    Light = 0,
    Regular = 1,
    Heavy = 2,
}
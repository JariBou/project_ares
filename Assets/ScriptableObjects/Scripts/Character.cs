using System;
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
        
        
        
        
        public void ApplyPreset(Character preset)
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
    None =3,
    Light = 0,
    Regular = 1,
    Heavy = 2,
}
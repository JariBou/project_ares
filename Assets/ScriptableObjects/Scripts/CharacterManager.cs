using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CharacterManager")]
    public class CharacterManager : ScriptableObject
    {
        [SerializeField] private List<Character> _charactersList;
        
        [Space, Header("Character Creation")]

        [Expandable]
        public Character _newCharacter;

        public int Count => _charactersList.Count;

        #if UNITY_EDITOR
        [Button("Create Character Asset from New Character")]
        public void CreateNewCharacter()
        {
            if (_newCharacter == null) {Debug.LogWarning("Trying to create a new Character from void"); return;}
            
            Character asset = CreateInstance<Character>();
            asset.ApplyPreset(_newCharacter);
            _charactersList.Add(asset);
            
            AssetDatabase.CreateAsset(asset, $"Assets/ScriptableObjects/Objects/Characters/{asset._name}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
        
        [Button("Start with Blank Character")]
        public void StartNewCharacter()
        {
            _newCharacter = CreateInstance<Character>();
        }

        #region Presets

        [SerializeField]
        private Character _lightPreset ;
        
        [SerializeField]
        private Character _regularPreset;
        
        [SerializeField]
        private Character _heavyPreset;
        
        [Button("Start with Light Preset")]
        public void LightPreset()
        {
            _newCharacter = _lightPreset;
        }

        [Button("Start with Regular Preset")]
        public void RegularPreset()
        {
            _newCharacter = _regularPreset;
        }

        [Button("Start with Heavy Preset")]
        public void HeavyPreset()
        {
            _newCharacter = _heavyPreset;
        }
        #endregion
        #endif

        public Character this[int selectionIndex] => _charactersList[selectionIndex];
    }
}

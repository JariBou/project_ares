using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ColorSelection")]
    public class ColorSelection : ScriptableObject
    {
        public List<Color> _possibleColors;
    }
}

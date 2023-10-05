using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AttackCombo")]
    public class AttackComboSO : ScriptableObject
    {
        public List<AttackSo> Attacks;
        public int Count => Attacks.Count;
    }
}

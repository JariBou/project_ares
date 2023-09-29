using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Attack")]
    public class AttackSo : ScriptableObject
    {
        public int Damage;
        public int KbAmount;
        public int IFramesCount;
        public int BlockMoveFramesCount;
        
    }
}

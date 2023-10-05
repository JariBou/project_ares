using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Attack")]
    public class AttackSo : ScriptableObject
    {
        [FormerlySerializedAs("Damage")] public int _damage;
        [FormerlySerializedAs("KbAmount")] public int _kbAmount;
        [FormerlySerializedAs("IFramesCount")] public int _framesCount;
        [FormerlySerializedAs("BlockMoveFramesCount")] public int _blockMoveFramesCount;
        [FormerlySerializedAs("_AnimatorOverride")] public AnimatorOverrideController _animatorOverride;
        public ButtonPos _buttonPos;
    }
}

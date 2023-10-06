using ProjectAres.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectAres.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Attack")]
    public class AttackSo : ScriptableObject
    {
        [FormerlySerializedAs("Damage")] public int _damage;
        [FormerlySerializedAs("KbAmount")] public int _kbAmount;
        [FormerlySerializedAs("_framesCount")] public int _invincibilityFramesCount;
        [FormerlySerializedAs("BlockMoveFramesCount")] public int _blockMoveFramesCount;
        [FormerlySerializedAs("_AnimatorOverride")] public AnimatorOverrideController _animatorOverride;
    }
}

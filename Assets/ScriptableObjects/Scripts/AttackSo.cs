using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectAres.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Attack")]
    public class AttackSo : ScriptableObject
    {
        [Header("Regular Stats")]
        [FormerlySerializedAs("Damage"), HideIf("_isSpAtk")] public int _damage;
        [FormerlySerializedAs("KbAmount"), HideIf("_isSpAtk")] public int _kbAmount;
        [FormerlySerializedAs("_framesCount")] public int _invincibilityFramesCount;
        [FormerlySerializedAs("BlockMoveFramesCount")] public int _blockMoveFramesCount;
        [FormerlySerializedAs("_AnimatorOverride")] public AnimatorOverrideController _animatorOverride;
        public int _maxFramesBeforeNextInput = 10;
        
        [Header("Special attack part")]
        public bool _isSpAtk;
        [ShowIf("_isSpAtk")] public bool _isInstantCast;
        [HideIf("_isInstantCast")] public AnimatorOverrideController _startAnimatorOverride;
        [ShowIf("_isSpAtk")] public bool _hasEndAnimationCast;
        [ShowIf("_hasEndAnimationCast")] public AnimatorOverrideController _endAnimatorOverride;
        [ShowIf("_isSpAtk")] public GameObject _spAttackGameObject;
    }
}

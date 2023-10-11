using NaughtyAttributes;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public interface IDamageable
    {
        public int PlayerId { get; set; }
        public void ApplyKb(Vector2 force);
        public void SetIFrames(int frameCount);
        public void IsAttacked();
        public void SetBlockedFramesCount(int frameCount);
        public void OnPreUpdate();
        
        public IDamageable WithCharacter(int playerId);
        public IDamageable SetId(int playerId);
        public IDamageable SetCharacter(Character character);


    }
}

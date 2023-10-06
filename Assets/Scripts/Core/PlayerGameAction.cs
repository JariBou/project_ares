using UnityEngine;

namespace ProjectAres.Core
{
    public class PlayerGameAction
    {
        public int OwnerId { get; private set; } // Should be a player script
        // public Character Owner { get; private set; } // Should be a player script
        public int TargetId { get; private set; } // Should be a player script
        // public Character Target { get; private set; } // Should be a player script
        public PlayerActionType ActionType { get; private set; }
        public AttackStats AttackStats { get; private set; }
        
        public PlayerGameAction(int ownerId, int targetId, PlayerActionType actionType)
        {
            OwnerId = ownerId;
            TargetId = targetId;
            ActionType = actionType;
        }
        
        public PlayerGameAction(int ownerId, int targetId, PlayerActionType actionType, AttackStats attackStats)
        {
            OwnerId = ownerId;
            TargetId = targetId;
            ActionType = actionType;
            AttackStats = attackStats;
        }
    }

    public class AttackStats
    {
        public int Damage { get; private set; }
        public int KbValue { get; private set; } // Strength of KB basically
        public int InvincibilityFrames { get; private set; } // Number of frames the ennemy cant be hit again
        public int MoveBlockFrames { get; private set; } // Number of frames the ennemy cant move
        public Vector2 ForceDirection { get; private set; }
        
        public AttackStats(int damage, int kbValue, Vector2 forceDirection, int invincibilityFrames, int moveBlockFrames)
        {
            Damage = damage;
            KbValue = kbValue;
            ForceDirection = forceDirection;
            InvincibilityFrames = invincibilityFrames;
            MoveBlockFrames = moveBlockFrames;
        }
        
    }

    // Order of enum is order of execution
    public enum PlayerActionType
    {
        Block,
        Attack,
        IFrames,
        MoveBlockFrames
    }
}

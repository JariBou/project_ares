using ScriptableObjects.Scripts;

namespace Core
{
    public class PlayerGameAction
    {
        public int OwnerId { get; private set; } // Should be a player script
        // public Character Owner { get; private set; } // Should be a player script
        public int TargetId { get; private set; } // Should be a player script
        // public Character Target { get; private set; } // Should be a player script
        public PlayerActionType ActionType { get; private set; }
        
        public PlayerGameAction(int ownerId, int targetId, PlayerActionType actionType)
        {
            OwnerId = ownerId;
            TargetId = targetId;
            ActionType = actionType;
        }
    }

    // Order of enum is order of execution
    public enum PlayerActionType
    {
        Block,
        Attack
    }
}

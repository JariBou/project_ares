namespace Core
{
    public class PlayerGameAction
    {
        public int OwnerId { get; private set; } // Should be a player script
        public int TargetId { get; private set; } // Should be a player script
        public PlayerActionType ActionType { get; private set; }

        public PlayerGameAction(int ownerId, int targetId, PlayerActionType actionType)
        {
            OwnerId = ownerId;
            TargetId = targetId;
            ActionType = actionType;
        }
    }

    public enum PlayerActionType
    {
        Block,
        Attack
    }
}

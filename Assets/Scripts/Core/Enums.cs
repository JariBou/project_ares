namespace ProjectAres.Core
{
    public enum ButtonPos
    {
        South,
        North,
        East,
        West,
        TriggerR2
    }
    
    public enum CharacterType
    {
        None =3,
        Light = 0,
        Regular = 1,
        Heavy = 2,
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

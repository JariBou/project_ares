using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HurtBox : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        public PlayerCharacterOld OwnerOld { get; private set;}
        public Damageable OwnerOld2 { get; private set;}
        public IDamageable Owner { get; private set;}

        public Animator Animator => _animator;

        public void SetOwner(PlayerCharacterOld playerCharacter)
        {
            OwnerOld2 = playerCharacter;
        }
        
        public void SetOwner(Damageable damageable)
        {
            OwnerOld2 = damageable;
        }
        
        public void SetOwner(IDamageable damageable)
        {
            Owner = damageable;
        }
        // TODO: There should be a list of all the hurtboxes of the character in a script
        // TODO: Also that should be used to set the link to playerCharacter for health and stuff
    }
}

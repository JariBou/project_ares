using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HurtBox : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        public PlayerCharacter OwnerOld { get; private set;}
        public Damageable Owner { get; private set;}

        public Animator Animator => _animator;

        public void SetOwner(PlayerCharacter playerCharacter)
        {
            OwnerOld = playerCharacter;
        }
        
        public void SetOwner(Damageable damageable)
        {
            Owner = damageable;
        }
        
        // TODO: There should be a list of all the hurtboxes of the character in a script
        // TODO: Also that should be used to set the link to playerCharacter for health and stuff
    }
}

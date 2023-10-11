using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HurtBox : MonoBehaviour
    {
        public Damageable Owner { get; private set;}
        
        public void SetOwner(Damageable damageable)
        {
            Owner = damageable;
        }
    }
}

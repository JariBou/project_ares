using System;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HurtBox : MonoBehaviour
    {
        public Damageable Owner { get; private set;}

        public Collider2D Collider => _collider;

        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void SetOwner(Damageable damageable)
        {
            Owner = damageable;
        }
    }
}

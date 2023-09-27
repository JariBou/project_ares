using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres
{
    public class HurtBox : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public Animator Animator => _animator;
    }
}

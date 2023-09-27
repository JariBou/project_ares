using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres
{
    public class DummyScript : MonoBehaviour
    {
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
    }
}

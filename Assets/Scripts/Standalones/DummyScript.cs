using UnityEngine;

namespace ProjectAres.Standalones
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

using Core;
using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAres.PlayerBundle
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce = 6f;
        private Rigidbody2D _rb;

        public Vector2 MoveVector { get; private set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    
        public void Move(InputAction.CallbackContext context)
        {
            MoveVector = context.ReadValue<Vector2>().normalized * _speed;
        }
    
        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.performed) {return;}
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }

        public void ButtonSouth(InputAction.CallbackContext context)
        {
            if (!context.performed) {return;}
            EffectsManager.StartShockwave(transform.position, 1f);
        }

        // TODO: Implement ActionStack to define actions made in each frame
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EffectsManager.SpawnHitParticles(other.GetContact(0).point);
            }
        }

        private void OnEnable()
        {
            TickManager.FrameUpdate += OnFrameUpdate;
        }

        private void OnFrameUpdate()
        {
            _rb.velocity = new Vector2(MoveVector.x, _rb.velocity.y);
        }

        private void OnDisable()
        {
            TickManager.FrameUpdate -= OnFrameUpdate;
        }

        public void SetCharacterStats(Character character)
        {
            _speed = character._speed;
            _jumpForce = character._jumpForce;
        }
    }
}

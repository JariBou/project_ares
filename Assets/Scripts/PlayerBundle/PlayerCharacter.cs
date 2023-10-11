using NaughtyAttributes;
using ProjectAres.Core;
using ProjectAres.Managers;
using ProjectAres.ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class PlayerCharacter : MonoBehaviour, IDamageable
    {
        [SerializeField] protected HurtBoxesManager _hurtBoxesManager;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        public PlayerInputHandler InputHandler => _playerInputHandler;

        [SerializeField] protected TMP_Text _text;
        [SerializeField, Expandable] protected Character _character;
        [SerializeField] protected Animator _animator;
        protected Rigidbody2D _rb;
        public Animator Animator => _animator;
        public Character Character => _character;

        protected int IFramesCount;
        protected int BlockedFramesCount;
        public bool IsInvincible { get; protected set; }

        private BoxCollider2D _pushBox;
        
        #if UNITY_EDITOR
        [SerializeField, Foldout("Ground Check")] private Vector2 _groundCheckOffset;
        [SerializeField, Foldout("Ground Check")] private Vector2 _groundCheckSize;
        [SerializeField, Foldout("Ground Check")] private bool _showGroundDetection;
        [SerializeField, Foldout("Ground Check"), ShowIf("_showGroundDetection")] private Color _groundCheckColor;
        #endif
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _text.text = _character._name;
            _hurtBoxesManager.SetOwners(this);
            InputHandler.SetCharacter(_character);
        }


        public int PlayerId { get; set; }

        public void ApplyKb(Vector2 force)
        {
            _rb.velocity = force;
        }

        public void SetIFrames(int frameCount)
        {
            IFramesCount = frameCount;
            IsInvincible = IFramesCount > 0;
        }

        public void IsAttacked()
        {
            InputHandler.StopAttacking();
        }

        public void SetBlockedFramesCount(int frameCount)
        {
            BlockedFramesCount = frameCount;
            InputHandler.SetMoveState(BlockedFramesCount == 0);
        }

        public void OnPreUpdate()
        {
            IsInvincible = IFramesCount > 0;
            IFramesCount = IFramesCount > 0 ? IFramesCount-1 : 0;
            BlockedFramesCount = BlockedFramesCount > 0 ? BlockedFramesCount-1 : 0;
            InputHandler.SetMoveState(BlockedFramesCount == 0);
        }

        public IDamageable WithCharacter(int playerId)
        {
            PlayerId = playerId;
            _character = PlayerManager.Instance.GetCharacterOfPlayer(playerId);
            return this;
        }

        public IDamageable SetId(int playerId)
        {
            PlayerId = playerId;
            return this;
        }

        public IDamageable SetCharacter(Character character)
        {
            _character = character;
            return this;
        }
        
        private void OnEnable()
        {
            TickManager.PreUpdate += OnPreUpdate;
        }

        private void OnDisable()
        {
            TickManager.PreUpdate -= OnPreUpdate;
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            _character._groundCheckOffset = _groundCheckOffset;
            _character._groundCheckSize = _groundCheckSize;
        }

        private void OnDrawGizmos()
        {
            if (_showGroundDetection)
            {
                Gizmos.color = _groundCheckColor;
                Gizmos.DrawWireCube((Vector2)transform.position + _groundCheckOffset, _groundCheckSize);
            }
        }
        #endif

    }
}

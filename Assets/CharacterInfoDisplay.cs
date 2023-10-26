using System;
using ProjectAres.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAres
{
    public class CharacterInfoDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingHpPercent;
        [SerializeField] private Image _characterImage;

        private Damageable _linkedPlayerCharacter;

        private void Start()
        {
            _remainingHpPercent.color =
                _linkedPlayerCharacter.Manager.GetColorOfPlayer(_linkedPlayerCharacter.PlayerId);
        }

        public void SetPlayerCharacter(Damageable playerCharacter)
        {
            _linkedPlayerCharacter = playerCharacter;
            _characterImage.sprite = _linkedPlayerCharacter.Character._characterIcon;
        }
        
        
        private void UpdateDisplay()
        {
            _remainingHpPercent.text = $"{Math.Round(_linkedPlayerCharacter.GetPlayerPercentRemainingHp() * 100f, 2)}%";
        }

        private void OnEnable()
        {
            TickManager.PostUpdate += UpdateDisplay;
        }
        
        private void OnDisable()
        {
            TickManager.PostUpdate -= UpdateDisplay;
        }

    }
}

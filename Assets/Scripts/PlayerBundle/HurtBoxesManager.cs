using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HurtBoxesManager : MonoBehaviour
    {
        [SerializeField] private List<HurtBox> _hurtBoxes;
        public List<HurtBox> HurtBoxes => _hurtBoxes;

        public void SetOwners(Damageable owner)
        {
            foreach (HurtBox hurtBox in _hurtBoxes)
            {
                hurtBox.SetOwner(owner);
            }
        }

        [Button]
        public void GetAllHurtBoxes()
        {
            HurtBox[] boxes = GetComponentsInChildren<HurtBox>();
            _hurtBoxes = new List<HurtBox>(boxes.Length);
            foreach (HurtBox hurtBox in boxes)
            {
                _hurtBoxes.Add(hurtBox);
            }
        }
    }
}

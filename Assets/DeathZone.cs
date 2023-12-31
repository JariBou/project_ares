using ProjectAres.PlayerBundle;
using UnityEngine;

namespace ProjectAres
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("HurtBox"))
            {
                other.GetComponent<HurtBox>().Owner.Respawn();
            }
        }
    }
}

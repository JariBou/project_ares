using UnityEngine;

namespace ProjectAres.PlayerBundle
{
    public class HitBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Collided with {other.gameObject.name}");
            if (other.gameObject.CompareTag("Dummy"))
            {
                other.GetComponent<HurtBox>().Animator.SetTrigger("Hurt");
            }
        }
    }
}

using System.Collections;
using UnityEngine;

namespace ProjectAres.Core
{
    public class EffectsManager : MonoBehaviour
    {
        private static readonly int WaveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

        [SerializeField] private GameObject _shockwavePrefab;
        [SerializeField] private GameObject _hitParticlesPrefab;
        private static EffectsManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        private IEnumerator ShockWaveAction(Vector2 position, float shockwaveTime, float shockwaveSize = 0.05f)
        {
            SpriteRenderer spriteRenderer =
                Instantiate(_shockwavePrefab, position, Quaternion.identity).GetComponent<SpriteRenderer>();
            Material mat = spriteRenderer.material;
            float lerpedAmount = 0f;
            mat.SetFloat("Size", shockwaveSize);

            float elapsedTime = 0f;
            while (elapsedTime < shockwaveTime)
            {
                elapsedTime += Time.deltaTime;

                lerpedAmount = Mathf.Lerp(-0.1f, 1f, elapsedTime / shockwaveTime);
                mat.SetFloat(WaveDistanceFromCenter, lerpedAmount);

                yield return null;
            }
        
            Destroy(spriteRenderer.gameObject);
        }
    
        // TODO: Honestly not a huge fan of all of this, we'll see when we have more effects
        public static void StartShockwave(Vector2 position, float shockwaveTime)
        {
            Instance.StartCoroutine(Instance.ShockWaveAction(position, shockwaveTime));
        }
        
        public static void StartShockwave(Vector2 position, float shockwaveTime, float shockwaveSize)
        {
            Instance.StartCoroutine(Instance.ShockWaveAction(position, shockwaveTime, shockwaveSize));
        }
        
        public static void SpawnHitParticles(Vector2 point)
        {
            Instantiate(Instance._hitParticlesPrefab, point, Quaternion.identity).GetComponent<ParticleSystem>();
        }
    }
}

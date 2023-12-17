namespace RPG.Core
{
    using UnityEngine;

    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject effect = null;
        ParticleSystem effectParticleSystem = null;
        private void Awake()
        {
            if (effect)
            {
                effectParticleSystem = effect.GetComponent<ParticleSystem>();
            }
        }

        private void Update()
        {
            if (effectParticleSystem && !effectParticleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
namespace GameOfWar.Characters
{
    using System;
    using RPG.Core;
    using RPG.Saving;
    using RPG.Stats;
    using UnityEngine;
    using GameDevTV.Utils;
    using UnityEngine.Events;

    public class Health : MonoBehaviour
    {
        [SerializeField] TakeDamageEvent takeDamageEvent;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        public float healthPoint;
        float damageTaken = 0f;
        public bool isDead = false;
        [SerializeField] UnityEvent onDieEvent;

        BaseStats baseStats;
        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();

        }
        private void Start()
        {
        }

        public void RegenerateHealth()
        {

        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            damageTaken = damage;
            healthPoint = Mathf.Max(healthPoint - damage, 0);
            takeDamageEvent.Invoke(damage);
            if (healthPoint == 0)
            {
                onDieEvent.Invoke();
                Die(instigator);
            }
            print(gameObject.name + " took damage: " + damageTaken);
        }
        public float GetHealthPoint()
        {
            return healthPoint;
        }

        public float GetPercentageHealh()
        {
            return healthPoint / baseStats.GetStat(Stat.Health) * 100;
        }
        void Die(GameObject instigator)
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            if (GetComponent<Collider>() != null)
            {
                GetComponent<Collider>().enabled = false;
            }
            //GetComponent<ActionScheduler>().CancelCurrentAction();
            //AwardExperience(instigator);

        }

        private void AwardExperience(GameObject instigator)
        {

            if (instigator == null) return;
            Experience exp = instigator.GetComponent<Experience>();
            if (exp == null) return;
            exp.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

    }
}
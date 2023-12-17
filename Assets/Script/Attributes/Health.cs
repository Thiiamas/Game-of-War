namespace RPG.Attributes
{
    using System;
    using RPG.Core;
    using RPG.Saving;
    using RPG.Stats;
    using UnityEngine;
    using GameDevTV.Utils;
    using UnityEngine.Events;

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] TakeDamageEvent takeDamageEvent;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoint;
        float damageTaken = 0f;
        public bool isDead = false;
        [SerializeField] UnityEvent onDieEvent;

        BaseStats baseStats;
        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            healthPoint = new LazyValue<float>(GetInitialHealth);

        }
        private void Start()
        {
            healthPoint.ForceInit();
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStats.OnLevelUpEvent += RegenerateHealth;
        }

        private void OnDisable()
        {
            baseStats.OnLevelUpEvent -= RegenerateHealth;

        }
        public void RegenerateHealth()
        {
            healthPoint.value = baseStats.GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            damageTaken = damage;
            healthPoint.value = Mathf.Max(healthPoint.value - damage, 0);
            takeDamageEvent.Invoke(damage);
            if (healthPoint.value == 0)
            {
                onDieEvent.Invoke();
                Die(instigator);
            }
            print(gameObject.name + " took damage: " + damageTaken);
        }
        public float GetHealthPoint()
        {
            return healthPoint.value;
        }

        public float GetMaxHealthPoint()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetPercentageHealh()
        {
            return healthPoint.value / baseStats.GetStat(Stat.Health) * 100;
        }
        void Die(GameObject instigator)
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            AwardExperience(instigator);

        }

        private void AwardExperience(GameObject instigator)
        {

            if (instigator == null) return;
            Experience exp = instigator.GetComponent<Experience>();
            if (exp == null) return;
            exp.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoint.value;
        }

        public void RestoreState(object state)
        {
            healthPoint.value = (float)state;
            if (healthPoint.value == 0)
            {
                Die(null);
            }
        }

        internal float GetFraction()
        {
            return healthPoint.value / GetMaxHealthPoint();
        }

        internal bool isFullLife()
        {
            if (healthPoint.value == GetMaxHealthPoint())
            {
                return true;
            }
            return false;
        }
    }
}
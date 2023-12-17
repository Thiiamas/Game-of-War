using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using System;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using GameDevTV.Utils;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBeetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        Animator animator;
        Mover mover;
        [SerializeField] WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
        }

        private Weapon SetDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);

        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            currentWeapon.value = AttachWeapon(weaponConfig);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.isDead) return;
            if (target != null && !GetIsInRange(target.transform))
            {
                mover.MoveTo(target.transform.position, 1);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }
        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= currentWeaponConfig.GetWeaponTimeBetweenAttack())
            {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            transform.LookAt(combatTarget.transform);
            target = combatTarget.GetComponent<Health>();
        }
        public bool CanAttack(GameObject targetToTest)
        {
            if (targetToTest == null) return false;

            if (!mover.CanMove(targetToTest.transform.position) && !GetIsInRange(targetToTest.transform)) return false;
            Health targetHealth = targetToTest.GetComponent<Health>();
            return targetHealth != null && !targetHealth.isDead;

        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeaponConfig.hasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
            print("CR" + currentWeapon.value);
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
        }

        public IEnumerable GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }
        public IEnumerable GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonusDamage();
            }
        }

        private void HitTarget()
        {
        }

        //Animation Event
        void Shoot()
        {
            Hit();
        }

        public void Cancel()
        {
            target = null;
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            EquipWeapon(Resources.Load<WeaponConfig>((string)state));
        }


    }
}
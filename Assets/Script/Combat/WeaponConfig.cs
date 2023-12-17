namespace RPG.Combat
{
    using System.Dynamic;
    using System.Runtime.InteropServices;
    using RPG.Attributes;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentageBonusDamage = 0f;
        [SerializeField] float weaponAttackSpeed = 1f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon equippedWeapon = null;
            if (equippedPrefab)
            {
                equippedWeapon = Instantiate(equippedPrefab, GetTransform(rightHand, leftHand));
                equippedWeapon.gameObject.name = weaponName;
            }
            if (animatorOverride) animator.runtimeAnimatorController = animatorOverride;
            return equippedWeapon;
        }

        void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (!oldWeapon) oldWeapon = leftHand.Find(weaponName);
            if (!oldWeapon) return;
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetPercentageBonusDamage()
        {
            return percentageBonusDamage;
        }
        public float GetWeaponTimeBetweenAttack()
        {
            return 1 / weaponAttackSpeed;
        }
        public bool hasProjectile()
        {
            return projectile != null;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            if (!projectile)
            {
                Debug.Log("Trying to shoot with No projectile");
                return;
            }
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(instigator, target, calculatedDamage);
        }

        Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            if (isRightHanded) return rightHandTransform;
            else return leftHandTransform;
        }

    }
}
namespace GameOfWar.Game
{
    using System;
    using GameOfWar.Characters;
    using UnityEngine;
    using UnityEngine.Events;

    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] GameObject impactEffect = null;
        [SerializeField] Transform impactTransform = null;
        [SerializeField] float lifeAfterImpact = .3f;
        [SerializeField] Health target = null;
        [SerializeField] bool isHoming = true;
        [SerializeField] float maxLifeTime = 20f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] UnityEvent onHit;

        float damage = 0f;
        GameObject instigator = null;
        void Start()
        {
            transform.LookAt(GetAimLocation());

        }
        private void Update()
        {
            if (target == null) return;
            if (isHoming && !target.isDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject instigator, Health target, float damage)
        {
            this.damage = damage;
            this.target = target;
            this.instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (target == null) return transform.position;
            return target.transform.position + Vector3.up * 1f;
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            print("Projectile hit: " + other.gameObject.name);
            this.speed = 0f;
            if (impactEffect)
            {
                Vector3 impactPosition = impactTransform ? impactTransform.position : GetAimLocation();
                Instantiate(impactEffect, impactPosition, transform.rotation);
            }
            if (other.gameObject.GetComponent<Health>() != null)
            {
                other.gameObject.GetComponent<Health>().TakeDamage(instigator, damage);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
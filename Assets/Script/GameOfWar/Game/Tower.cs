namespace GameOfWar.Game
{
    using System;
    using System.Collections.Generic;
    using GameOfWar.Characters;
    using GameOfWar.Game;
    using RPG.Combat;
    using UnityEngine;
    public class Tower : MonoBehaviour
    {

        public Character target;
        public List<GameObject> enemiesGO;
        public List<Character> enemies;
        public GameObject bulletPrefab;
        public float Range = 10f;
        public float fireCounter = Mathf.Infinity;
        public float FireRate = 1f;
        public float FireCountdown = 0f;

        public string EnemyTag = "Enemy";

        public Transform PartToRotate;
        public float TurnSpeed = 10f;

        public Transform FirePoint;

        // Use this for initialization
        void Start()
        {
            enemies = new List<Character>();
            enemiesGO = new List<GameObject>();
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }

        private void Update()
        {
            fireCounter += Time.deltaTime;
            if (target == null)
            {
                return;
            }
            HandleShotting();

        }

        private void HandleShotting()
        {
            if (fireCounter >= FireRate)
            {
                Shoot();
                fireCounter = 0;
            }
        }

        private void Shoot()
        {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);

            Projectile projectile = bulletGO.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetTarget(gameObject, target.GetComponent<Health>(), 50f);
            }
        }

        public void UpdateTarget()
        {
            float shortestDistance = Mathf.Infinity;
            Character nearestEnemy = null;

            foreach (Character enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null)
            {
                target = nearestEnemy;
                if (target.GetComponent<Health>().isDead)
                {
                    enemies.Remove(target);
                    enemiesGO.Remove(target.gameObject);
                    target = null;
                    return;
                }
            }
            target = nearestEnemy;
        }

        public void addEnemy(GameObject enemy)
        {
            enemies.Add(enemy.gameObject.GetComponent<Character>());
            enemiesGO.Add(enemy);
        }
    }
}
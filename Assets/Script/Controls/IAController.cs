namespace RPG.Control
{
    using System;
    using GameDevTV.Utils;
    using RPG.Attributes;
    using RPG.Combat;
    using RPG.Core;
    using RPG.Movement;
    using UnityEngine;

    public class IAController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 10f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float agrevationMaxTime = 3f;
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter;
        ActionScheduler actionScheduler;
        Health health;
        GameObject player;
        Mover mover;
        [SerializeField] Collider collider = null;
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(GetStartPosition);
        }
        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.isDead && collider != null && collider.enabled)
            {
                collider.enabled = false;
                return;
            }
            if (health.isDead) return;
            if (ShouldChase(player) && fighter.CanAttack(player))
            {
                InteractWithCombat(player);
                timeSinceLastSawPlayer = 0;
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                timeSinceArrivedAtWaypoint = 0;
                PatrolBehaviour();
            }
            UpdateTimer();
        }

        public void Agrevate()
        {
            timeSinceAggrevated = 0;
        }
        private Vector3 GetStartPosition()
        {
            return transform.position;
        }





        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                guardPosition.value = GetCurrentWaypoint();
            }
            mover.StartMoveAction(guardPosition.value, patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }



        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            if (distanceToWaypoint <= waypointTolerance) return true;
            return false;
        }



        private void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        private void InteractWithCombat(GameObject player)
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);

            AgrevateNearbyEnemies();
        }

        private void AgrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                IAController enemy = hit.collider.GetComponent<IAController>();
                if (enemy == null) continue;
                enemy.Agrevate();
            }
        }

        private bool ShouldChase(GameObject player)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < agrevationMaxTime;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

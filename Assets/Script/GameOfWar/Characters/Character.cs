namespace GameOfWar.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;


    public class Character : MonoBehaviour
    {
        public GameObject objective;
        private NavMeshAgent agent;
        private Animator animator;
        private Health health;

        // Start is called before the first frame update
        void Start()
        {
            objective = GameObject.FindGameObjectWithTag("Objective");
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.isDead)
            {
                agent.enabled = false;
                return;
            }
            if (animator != null)
            {
                animator.SetFloat("ForwardSpeed", agent.velocity.magnitude);
            }
            if (objective != null)
            {
                agent.SetDestination(objective.transform.position);
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using System;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Health health;
        // Start is called before the first frame update
        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.isDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float fractionSpeed)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, fractionSpeed);
        }

        public bool CanMove(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }


        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float zSpeed = localVelocity.z;
            animator.SetFloat("ForwardSpeed", zSpeed);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }


        public void MoveTo(Vector3 destination, float fractionSpeed)
        {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp(fractionSpeed, 0, 1);
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total = +Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }

}
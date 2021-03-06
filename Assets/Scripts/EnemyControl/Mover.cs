using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ETD.PathFinding;
using ETD.PlayerControl;

namespace ETD.EnemyControl
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float closeEnoughToWaypoint = .5f;
        [SerializeField] bool isPathTester = false;
        [SerializeField] Pathing path;

        NavMeshAgent navMeshAgent;
        Waypoint[] waypoints;
        int nextWaypointIndex = 0;
        bool isFinished = false;
        bool isStunned = false;

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public void SetPath(Pathing newPath)
        {
            path = newPath;
        }

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = moveSpeed;
        }

        private void Update()
        {
            if(isPathTester) { return; }
            if(isStunned) { return; }
            Move();
        }

        private void Move()
        {
            if(path == null) { Debug.LogError("No Path!");  return; }
            if(isFinished) { return; }
            if(waypoints == null) { waypoints = path.GetPath(); }
            Transform nextWaypoint = waypoints[nextWaypointIndex].transform;
            navMeshAgent.destination = nextWaypoint.position;
            if(Vector3.Distance(transform.position, nextWaypoint.position) <= closeEnoughToWaypoint)
            {
                nextWaypointIndex++;
                if(nextWaypointIndex == waypoints.Length)
                {
                    MadeItToEnd();
                }
            }
        }

        private void MadeItToEnd()
        {
            isFinished = true;
            Debug.Log("Finished path!");
            FindObjectOfType<Lives>().LoseLife();
            GetComponent<Health>().Die();
        }

        public bool IsPathBlocked()
        {
            waypoints = path.GetPath();
            foreach(Waypoint waypoint in waypoints)
            {
                NavMeshPath path = new NavMeshPath();
                navMeshAgent.CalculatePath(waypoint.transform.position, path);
                if(path.status == NavMeshPathStatus.PathPartial)
                {
                    return true;
                }
            }
            return false;
        }

        public void ApplyStatusEffect(AbilitiesAndStatusEffects effect)
        {
            if (Mathf.RoundToInt(Random.Range(0.5f, effect.GetEffectChance() + (0.5f - Mathf.Epsilon))) == 1)
            {
                isStunned = true;
                navMeshAgent.ResetPath();
                StartCoroutine(StunTimer(effect));
            }
            else { GetComponent<DefenceApplicator>().RemoveStatusEffect(effect.GetStatusEffect()); }
        }

        IEnumerator StunTimer(AbilitiesAndStatusEffects effect)
        {
            yield return new WaitForSeconds(effect.GetStatusEffectLifeTime());
            isStunned = false;
            GetComponent<DefenceApplicator>().RemoveStatusEffect(effect.GetStatusEffect());
        }

    }
}

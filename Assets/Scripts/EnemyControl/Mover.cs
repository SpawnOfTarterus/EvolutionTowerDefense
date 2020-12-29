using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ETD.PathFinding;

namespace ETD.EnemyControl
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float closeEnoughToWaypoint = .5f;

        Pathing path;
        NavMeshAgent navMeshAgent;
        Waypoint[] waypoints;
        int nextWaypointIndex = 0;
        bool isFinished = false;

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
            Move();
        }

        private void Move()
        {
            if(path == null) { Debug.Log("No Path!");  return; }
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
            Destroy(gameObject);
        }

    }
}

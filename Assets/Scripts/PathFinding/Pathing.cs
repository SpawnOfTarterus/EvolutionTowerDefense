using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.PathFinding
{
    public class Pathing : MonoBehaviour
    {
        [SerializeField] Waypoint[] waypoints = new Waypoint[0];

        public Waypoint[] GetPath()
        {
            return waypoints;
        }

        [ExecuteAlways]
        private void Update()
        {
            DrawGizmoToConnectWaypoints();
        }

        private void DrawGizmoToConnectWaypoints()
        {
           foreach(Waypoint waypoint in waypoints)
           {

           }
        }
    }
}

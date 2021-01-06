using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.BuildingControl
{
    [ExecuteInEditMode]
    public class GridControl : MonoBehaviour
    {
        [SerializeField] bool showGrid = false;
        [SerializeField] int gridSize = 10;
        [SerializeField] float gridSpacing = 0.5f;

        int valueRetention;

        public void SetShowGrid(bool setter)
        {
            showGrid = setter;
        }

        private void Update()
        {
            SnapMechanic();
        }

        private void OnDrawGizmos()
        {
            if(!showGrid) { return; }
            Vector3 startingPoint = transform.position - new Vector3(gridSize / 2, -0.1f, gridSize / 2);
            for(int i = 0; i < gridSize / gridSpacing + 1; i++ )
            {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(startingPoint + new Vector3(i * gridSpacing, 0, 0), 
                    startingPoint + new Vector3(i * gridSpacing, 0, gridSize));
            }
            for (int i = 0; i < gridSize / gridSpacing + 1; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(startingPoint + new Vector3(0, 0, i * gridSpacing), 
                    startingPoint + new Vector3(gridSize, 0, i * gridSpacing));
            }
        }

        private void SnapMechanic()
        {
            Vector3 posToSnap = transform.position;
            float newX = RoundToGrid(posToSnap.x) + ((int)posToSnap.x);
            float newY = RoundToGrid(posToSnap.y) + ((int)posToSnap.y);
            float newZ = RoundToGrid(posToSnap.z) + ((int)posToSnap.z);
            transform.position = new Vector3(newX, newY, newZ);
        }

        private float RoundToGrid(float vector)
        {
            float roundingNumber = gridSpacing / 2;
            float fractionAmount;
            if(vector < 0) 
            { 
                fractionAmount = Mathf.Abs(vector - (int)vector);
                valueRetention = -1;
            }
            else 
            { 
                fractionAmount = vector - (int)vector;
                valueRetention = 1;
            }
            if (fractionAmount < gridSpacing)
            {
                if (fractionAmount < roundingNumber )
                {
                    return 0f * valueRetention;
                }
                return gridSpacing * valueRetention;
            }
            if (fractionAmount < gridSpacing + roundingNumber)
            {
                return gridSpacing * valueRetention;
            }
            return 1f * valueRetention;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETD.TowerControl;
using ETD.EnemyControl;

namespace ETD.BuildingControl
{
    public class Building : MonoBehaviour
    {
        [SerializeField] int cost;
        [SerializeField] bool isBuilt = false;

        BuildingSpawner mySpawner;

        public int GetCost()
        {
            return cost;
        }

        public void SetSpawner(BuildingSpawner newSpawner)
        {
            mySpawner = newSpawner;
        }

        public void SetAsBuilt()
        {
            isBuilt = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (isBuilt) { return; }
            if (other.GetComponent<Tower>() || other.GetComponent<Enemy>() || other.GetComponent<Building>())
            {
                mySpawner.ToggleColorBuildIndicator(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isBuilt) { return; }
            if (other.GetComponent<Tower>() || other.GetComponent<Enemy>() || other.GetComponent<Building>())
            {
                mySpawner.ToggleColorBuildIndicator(true);
            }
        }
    }
}

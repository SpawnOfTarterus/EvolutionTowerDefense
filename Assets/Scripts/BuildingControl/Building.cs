using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETD.TowerControl;
using ETD.EnemyControl;
using ETD.UIControl;

namespace ETD.BuildingControl
{
    public class Building : MonoBehaviour
    {
        [SerializeField] bool isBuilt = false;
        [SerializeField] GameObject buildIndicator = null;

        int costToBuild;

        BuildingSpawner mySpawner;

        public int GetCost()
        {
            return costToBuild;
        }

        public void SetSpawner(BuildingSpawner newSpawner)
        {
            mySpawner = newSpawner;
        }

        public void SetAsBuilt()
        {
            isBuilt = true;
        }

        private void Start()
        {
            costToBuild = GetComponent<UISelectionDescription>().GetBuildCost();
        }

        private void Update()
        {
            buildIndicator.SetActive(!isBuilt);
        }

        private void OnTriggerStay(Collider other)
        {
            if (isBuilt) { return; }
            if (other.GetComponent<Tower>() || other.GetComponent<Enemy>() || other.GetComponent<Building>())
            {
                Debug.Log("Collision in progress - " + other.name);
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

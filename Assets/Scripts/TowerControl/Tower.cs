using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] bool isBuilt = false;

        TowerSpawner mySpawner = null;

        public void SetSpawner(TowerSpawner newSpawner)
        {
            mySpawner = newSpawner;
        }

        public void SetAsBuilt()
        {
            isBuilt = true;
        }

        private void Start()
        {
            
        }


        private void OnTriggerStay(Collider other)
        {
            if(isBuilt) { return; }
            Debug.Log("Collision in progress.");
            if (other.GetComponent<Tower>() || other.GetComponent<Enemy>())
            {
                mySpawner.ToggleColorBuildIndicator(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(isBuilt) { return; }
            Debug.Log("exit collision.");
            if (other.GetComponent<Tower>() || other.GetComponent<Enemy>())
            {
                mySpawner.ToggleColorBuildIndicator(true);
            }
        }




    }
}

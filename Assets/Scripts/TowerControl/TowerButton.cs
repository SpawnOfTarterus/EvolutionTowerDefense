using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ETD.TowerControl
{
    public class TowerButton : MonoBehaviour
    {
        [SerializeField] Tower towerPrefab = null;

        TowerSpawner towerSpawner;

        private void Start()
        {
            towerSpawner = FindObjectOfType<TowerSpawner>();
        }

        public void SelectTowerToSpawn()
        {
            towerSpawner.DisplayTowerToSpawn(towerPrefab);
        }

    }
}

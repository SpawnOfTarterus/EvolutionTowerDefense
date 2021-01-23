using ETD.BuildingControl;
using ETD.EnemyControl;
using ETD.PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] int costToEvolve;

        public int GetCost()
        {
            return costToEvolve;
        }

        public void Evolve(Tower newTower)
        {
            if(newTower.GetCost() <= FindObjectOfType<GoldController>().GetCurrentGold())
            {
                FindObjectOfType<BuildingSpawner>().EvolveSelectedTower(this, newTower);
            }
        }
    }
}

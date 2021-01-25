using ETD.BuildingControl;
using ETD.EnemyControl;
using ETD.PlayerControl;
using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Tower : MonoBehaviour
    {
        int costToEvolve;

        private void Start()
        {
            costToEvolve = GetComponent<UISelectionDescription>().GetBuildCost();
        }

        public void Evolve(Tower newTower)
        {
            if(newTower.costToEvolve <= FindObjectOfType<GoldController>().GetCurrentGold())
            {
                FindObjectOfType<BuildingSpawner>().EvolveSelectedTower(this, newTower);
                FindObjectOfType<GoldController>().SpendGold(costToEvolve);
            }
            else { Debug.Log("Not enough gold."); }
        }
    }
}

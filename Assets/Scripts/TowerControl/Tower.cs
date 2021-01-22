using ETD.EnemyControl;
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
 
    }
}

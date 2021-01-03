using ETD.WaveControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.EnemyControl
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] Transform hitTransform = null;
        [SerializeField] int goldReward = 1;

        EnemySpawner mySpawner;

        public Transform GetHitTransform()
        {
            return hitTransform;
        }

        public EnemySpawner GetMySpawner()
        {
            return mySpawner;
        }

        public void SetMySpawner(EnemySpawner spawner)
        {
            mySpawner = spawner;
        }

        public int GetGoldReward()
        {
            return goldReward;
        }
    }
}

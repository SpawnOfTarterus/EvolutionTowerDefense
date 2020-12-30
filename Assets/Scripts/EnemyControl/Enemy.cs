using ETD.WaveControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.EnemyControl
{
    public class Enemy : MonoBehaviour
    {
        EnemySpawner mySpawner;

        public void SetMySpawner(EnemySpawner spawner)
        {
            mySpawner = spawner;
        }

        public void Die()
        {
            mySpawner.RemoveFromEnemiesInPlay(this);
            Destroy(gameObject);
        }

    }
}

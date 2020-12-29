using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.WaveControl
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Wave/Make New Wave", order = 0)]
    public class Wave : ScriptableObject
    {
        [SerializeField] Enemy enemyPrefab = null;
        [SerializeField] int enemyCount = 0;
        [SerializeField] float spawnDelay = 1f;

        public Enemy GetEnemy()
        {
            return enemyPrefab;
        }

        public int GetEnemyCount()
        {
            return enemyCount;
        }

        public float GetSpawnDelay()
        {
            return spawnDelay;
        }
    }
}

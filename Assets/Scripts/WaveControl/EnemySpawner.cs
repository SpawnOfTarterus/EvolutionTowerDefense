using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETD.EnemyControl;
using ETD.PathFinding;

namespace ETD.WaveControl
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] Wave[] waves;
        [SerializeField] Pathing path;
        [SerializeField] float timeBetweenWaves = 10f;
        [SerializeField] List<Enemy> enemiesInPlay = new List<Enemy>();
        [SerializeField] Transform enemiesParent = null;

        int waveToSpawnIndex = 0;
        Wave currentWave;

        IEnumerator Start()
        {
            yield return SpawnWave(waveToSpawnIndex);
        }

        IEnumerator SpawnWave(int spawnIndex)
        {
            if(waves.Length == 0) { Debug.LogError("No wave to spawn. Add wave to spawner."); yield break; }
            currentWave = waves[spawnIndex];
            for(int i = 0; i < currentWave.GetEnemyCount(); i++)
            {
                yield return SpawnEnemy();           
            }
            while (enemiesInPlay.Count > 0)
            {
                yield return null;
            }
            Debug.Log("Wave Finished.");
            yield return SpawnNextWave();
        }

        IEnumerator SpawnNextWave()
        {
            if(waveToSpawnIndex < waves.Length - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
                waveToSpawnIndex++;
                StartCoroutine(SpawnWave(waveToSpawnIndex));
            }
            else
            {
                Debug.Log("All waves finished.");
            }
        }

        IEnumerator SpawnEnemy()
        {
            Enemy enemyToSpawn = currentWave.GetEnemy();
            Vector3 spawnPosition = path.GetPath()[0].transform.position;
            Enemy enemyInstance = Instantiate(enemyToSpawn, spawnPosition, transform.rotation, enemiesParent);
            enemyInstance.GetComponent<Mover>().SetPath(path);
            enemyInstance.SetMySpawner(this);
            enemiesInPlay.Add(enemyInstance);
            yield return new WaitForSeconds(currentWave.GetSpawnDelay());
        }

        public void RemoveFromEnemiesInPlay(Enemy enemy)
        {
            enemiesInPlay.Remove(enemy);
        }


    }
}

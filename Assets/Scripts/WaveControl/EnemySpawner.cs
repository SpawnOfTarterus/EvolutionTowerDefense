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

        int waveToSpawnIndex = 0;
        Wave currentWave;

        IEnumerator Start()
        {
            yield return SpawnWave(waveToSpawnIndex);
        }

        IEnumerator SpawnWave(int spawnIndex)
        {
            currentWave = waves[spawnIndex];
            for(int i = 0; i < currentWave.GetEnemyCount(); i++)
            {
                yield return SpawnEnemy();           
            }
            Debug.Log("Wave Finished,");
            yield return new WaitForSeconds(timeBetweenWaves);
            if(waveToSpawnIndex < waves.Length - 1)
            {
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
            Enemy enemyInstance = Instantiate(enemyToSpawn, spawnPosition, transform.rotation);
            enemyInstance.GetComponent<Mover>().SetPath(path);
            yield return new WaitForSeconds(currentWave.GetSpawnDelay());
        }


    }
}

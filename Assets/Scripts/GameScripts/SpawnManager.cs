using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Rate")]
    [SerializeField] private float spawnTime = 5f;

    [Header("Object References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject[] powerups;

    private bool stopSpawning = false;
    public int probablity = 0;

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnWaveFire());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (stopSpawning == false)
        {
            Vector3 postionToSpawn = new Vector3(Random.Range(-9f, 9f), 8, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, postionToSpawn, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (stopSpawning == false)
        {
            Vector3 postionToSpawn = new Vector3(Random.Range(-9f, 9f), 8, 0);
            int randomPowerup = Random.Range(0, 5);
            Instantiate(powerups[randomPowerup], postionToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }

    IEnumerator SpawnWaveFire()
    {
        while (stopSpawning == false)
        {
            probablity = Random.Range(1, 6);
            Vector3 postionToSpawn = new Vector3(Random.Range(-9f, 9f), 8, 0);
            if (probablity == 3)
            {
                Instantiate(powerups[5], postionToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3, 8));
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(3, 8));
            }
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
}

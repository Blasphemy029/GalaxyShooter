using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] GameObject explosionPrefab;
    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = GameObject.Find("Spawner").GetComponent<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            spawnManager.StartSpawning();
            Destroy(this.gameObject,0.25f);
        }
    }
}

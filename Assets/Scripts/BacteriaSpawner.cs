using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacteriaSpawner : MonoBehaviour
{
    public GameObject bacteriaPrefab; // El prefab de la bacteria que quieres instanciar.
    public int numberOfBacteriaToSpawn = 15; // La cantidad de bacterias que deseas generar.
    public Vector3 spawnAreaSize = new Vector3(20f, 0.5f, 20f); // El tamaño del área de generación.

    void Start()
    {
        SpawnBacteria();
    }

    void SpawnBacteria()
    {
        for (int i = 0; i < numberOfBacteriaToSpawn; i++)
        {
            Vector3 randomPosition = GetRandomSpawnPosition();
            Instantiate(bacteriaPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Genera una posición aleatoria dentro del área de generación.
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnAreaSize.y,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        // Asegúrate de que la posición esté dentro del área de generación.
        randomPosition += transform.position;

        return randomPosition;
    }
}

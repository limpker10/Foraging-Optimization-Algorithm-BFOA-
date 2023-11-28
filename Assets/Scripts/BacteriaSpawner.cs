using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacteriaSpawner : MonoBehaviour
{
    public GameObject bacteriaPrefab; // El prefab de la bacteria que quieres instanciar.
    public int numberOfBacteriaToSpawn = 15; // La cantidad de bacterias que deseas generar.
    public Vector3 spawnAreaSize = new Vector3(20f, 0.5f, 20f); // El tama�o del �rea de generaci�n.

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
        // Genera una posici�n aleatoria dentro del �rea de generaci�n.
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnAreaSize.y,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        // Aseg�rate de que la posici�n est� dentro del �rea de generaci�n.
        randomPosition += transform.position;

        return randomPosition;
    }
}

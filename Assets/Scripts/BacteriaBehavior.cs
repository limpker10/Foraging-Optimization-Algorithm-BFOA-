using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BacteriaBehavior : MonoBehaviour
{
    private GameObject playerObject;
    public float speed = 5f;
    public float detectionRange = 10f;
    public float reproductionDistance = 8f;
    public float reproductionRate = 2f;
    public float eliminationDistance = 15f; // Distancia para la eliminación
    public float lifeTime = 30f; // Tiempo de vida máximo
    private float reproductionTimer;
    private float lifeTimer;
    public float separationDistance = 2f; // Distancia deseada de separación
    public float separationStrength = 5f; // Fuerza de la separación
    private NavMeshAgent navMeshAgent;
    private float fastSpeed = 8f;
    // Debes tener una referencia al prefab que deseas clonar. Supongamos que tienes un prefab llamado "MiPrefab".
    public GameObject miPrefab;


    void Start()
    {
        // Encuentra el GameObject con el tag "player"
        playerObject = GameObject.FindGameObjectWithTag("Player");
        reproductionTimer = reproductionRate;
        lifeTimer = lifeTime;
        

        // Obtén el componente NavMeshAgent del prefab
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("El prefab de la bacteria debe tener un componente NavMeshAgent.");
        }
    }

    void Update()
    {
        
        float distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

        Vector3 separationForce = Vector3.zero;
        Collider[] hits = Physics.OverlapSphere(transform.position, separationDistance);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject) // Ignorar la propia bacteria
            {
                Vector3 directionAwayFromBacteria = transform.position - hit.transform.position;
                separationForce += directionAwayFromBacteria.normalized / directionAwayFromBacteria.magnitude;
            }
        }

        separationForce *= separationStrength;

        // Movimiento hacia el jugador si está dentro del rango de detección
        if (distanceToPlayer < detectionRange)
        {
            // Moverse hacia el jugador utilizando NavMeshAgent con velocidad rápida
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = fastSpeed; // Ajusta la velocidad rápida que desees
                navMeshAgent.SetDestination(playerObject.transform.position);
            }
        }
        else
        {
            // Genera una posición aleatoria dentro del NavMesh
            Vector3 randomDestination = GetRandomNavMeshPoint();

            // Establece la velocidad rápida
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = fastSpeed; // Ajusta la velocidad rápida que desees
                navMeshAgent.SetDestination(randomDestination);
            }

        }

        // Reproducción
        if (distanceToPlayer < reproductionDistance)
        {
            reproductionTimer -= Time.deltaTime;
            if (reproductionTimer <= 0)
            {
                Reproduce();
                reproductionTimer = reproductionRate;
            }
        }

        // Eliminación
        lifeTimer -= Time.deltaTime;
        if (distanceToPlayer > eliminationDistance && lifeTimer <= 0)
        {
            Destroy(gameObject);
        }

        transform.position += separationForce * Time.deltaTime;
    }


    void Reproduce()
    {
        // Calcula una nueva posición aleatoria dentro de una esfera de radio 2 unidades
        Vector3 newPosition = transform.position + Random.insideUnitSphere * 2;

        // Clona el objeto desde el prefab en la nueva posición con la rotación predeterminada
        Instantiate(gameObject, newPosition, Quaternion.identity);

        // Puedes hacer más acciones con el nuevo objeto si es necesario.
    }
    Vector3 GetRandomNavMeshPoint()
    {
        // Genera una posición aleatoria dentro del NavMesh
        NavMeshHit hit;
        Vector3 randomPoint = Vector3.zero;
        // Calcula una posición aleatoria dentro de una esfera de radio 20 unidades desde la posición actual de la bacteria
        Vector3 randomOffset = Random.insideUnitSphere * 20;
        if (NavMesh.SamplePosition(transform.position + randomOffset, out hit, 20.0f, NavMesh.AllAreas))
        {
            randomPoint = hit.position;
        }

        return randomPoint;
    }
}


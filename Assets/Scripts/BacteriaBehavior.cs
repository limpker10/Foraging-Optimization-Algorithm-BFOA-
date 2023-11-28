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
    public float eliminationDistance = 15f; // Distancia para la eliminaci�n
    public float lifeTime = 30f; // Tiempo de vida m�ximo
    private float reproductionTimer;
    private float lifeTimer;
    public float separationDistance = 2f; // Distancia deseada de separaci�n
    public float separationStrength = 5f; // Fuerza de la separaci�n
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
        

        // Obt�n el componente NavMeshAgent del prefab
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

        // Movimiento hacia el jugador si est� dentro del rango de detecci�n
        if (distanceToPlayer < detectionRange)
        {
            // Moverse hacia el jugador utilizando NavMeshAgent con velocidad r�pida
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = fastSpeed; // Ajusta la velocidad r�pida que desees
                navMeshAgent.SetDestination(playerObject.transform.position);
            }
        }
        else
        {
            // Genera una posici�n aleatoria dentro del NavMesh
            Vector3 randomDestination = GetRandomNavMeshPoint();

            // Establece la velocidad r�pida
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = fastSpeed; // Ajusta la velocidad r�pida que desees
                navMeshAgent.SetDestination(randomDestination);
            }

        }

        // Reproducci�n
        if (distanceToPlayer < reproductionDistance)
        {
            reproductionTimer -= Time.deltaTime;
            if (reproductionTimer <= 0)
            {
                Reproduce();
                reproductionTimer = reproductionRate;
            }
        }

        // Eliminaci�n
        lifeTimer -= Time.deltaTime;
        if (distanceToPlayer > eliminationDistance && lifeTimer <= 0)
        {
            Destroy(gameObject);
        }

        transform.position += separationForce * Time.deltaTime;
    }


    void Reproduce()
    {
        // Calcula una nueva posici�n aleatoria dentro de una esfera de radio 2 unidades
        Vector3 newPosition = transform.position + Random.insideUnitSphere * 2;

        // Clona el objeto desde el prefab en la nueva posici�n con la rotaci�n predeterminada
        Instantiate(gameObject, newPosition, Quaternion.identity);

        // Puedes hacer m�s acciones con el nuevo objeto si es necesario.
    }
    Vector3 GetRandomNavMeshPoint()
    {
        // Genera una posici�n aleatoria dentro del NavMesh
        NavMeshHit hit;
        Vector3 randomPoint = Vector3.zero;
        // Calcula una posici�n aleatoria dentro de una esfera de radio 20 unidades desde la posici�n actual de la bacteria
        Vector3 randomOffset = Random.insideUnitSphere * 20;
        if (NavMesh.SamplePosition(transform.position + randomOffset, out hit, 20.0f, NavMesh.AllAreas))
        {
            randomPoint = hit.position;
        }

        return randomPoint;
    }
}


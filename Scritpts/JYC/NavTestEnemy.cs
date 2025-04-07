using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavTestEnemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float outsideArea = 6f;
    [SerializeField] private float insideArea = 3f;

    Vector3 randomPoint;

    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(_target.position, _target.position);

        if (distanceToTarget > outsideArea)
        {
            Vector3 randomDestination = GetRandomDestination(transform.position, insideArea);
            MoveTo(randomDestination);
        }
        else
        {
            MoveTo(_target.position);
        }
    }

    private Vector3 GetRandomDestination(Vector3 origin, float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere.normalized * Random.Range(0f, radius);
        randomDir = randomPoint;
        randomPoint += origin;
        return new Vector3(randomDir.x, transform.position.y, randomDir.z);
    }

    private void MoveTo(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }
}
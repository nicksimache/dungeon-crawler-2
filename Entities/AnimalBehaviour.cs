using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround;

    [SerializeField]
    public GameObject poopPrefab;
    public List<GameObject> poopList = new List<GameObject>();

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    private float timeElapsedSincePoop = 0f;
    private float maxProbability = 0.9f;
    private float baseProbabilityIncrement = 0.01f;
    private float probabilityIncrement;

    //temp variable
    [SerializeField]
    public int gameLevel;


    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        updateProbabilityIncrement();

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        timeElapsedSincePoop += Time.deltaTime;

        float currentProbability = Mathf.Min(timeElapsedSincePoop * probabilityIncrement, maxProbability);

        if(Random.value < currentProbability)
        {
            //Poo
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void updateProbabilityIncrement()
    {
        probabilityIncrement = baseProbabilityIncrement * gameLevel;
    }

    private void poop()
    {
        GameObject poop = Instantiate(poopPrefab, transform.position, Quaternion.identity);

        Rigidbody rb = poop.GetComponent<Rigidbody>();

        poopList.Add(poop);

    }
}

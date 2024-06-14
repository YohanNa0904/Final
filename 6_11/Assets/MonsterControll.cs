using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterControll : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject player;
    public float hp = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }
    public void TakeDamage(float n)
    {
        hp -= n;
        Destroy(gameObject);
    }
}

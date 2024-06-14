using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoint; // ��������Ʈ ����
    public Transform playerTransform; // �÷��̾� Transform ����
    int m_CurrentWayPointIndex;

    void Start()
    {
        if (waypoint.Length > 0)
        {
            navMeshAgent.SetDestination(waypoint[0].position);
        }
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWayPointIndex = (m_CurrentWayPointIndex + 1) % waypoint.Length;
            navMeshAgent.SetDestination(waypoint[m_CurrentWayPointIndex].position);
        }
    }
}

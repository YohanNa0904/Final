using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public GameObject[] monsters; // �� ������������ ������ ���� �迭
    }

    public Stage[] stages; // ��� �������� ����
    public Transform spawnCenter; // ���� �߽� ��ġ
    public float spawnRadius = 50f; // ���� �ݰ�
    public float timeBetweenStages = 30f; // �� �������� ������ ��� �ð�
    public Transform playerTransform; // �÷��̾� Transform ����
    public Transform[] waypoints; // ��������Ʈ ����

    private int currentStageIndex = 0;
    private List<GameObject> currentMonsters = new List<GameObject>();

    void Start()
    {
        StartCoroutine(StartNextStage());
    }

    IEnumerator StartNextStage()
    {
        while (currentStageIndex < stages.Length)
        {
            // ���� ���������� ���� ����
            SpawnMonsters(stages[currentStageIndex]);

            // ��� ���Ͱ� ���� ������ ���
            while (currentMonsters.Count > 0)
            {
                currentMonsters.RemoveAll(monster => monster == null);
                yield return null;
            }

            // ���� ���������� �Ѿ�� �� ���
            yield return new WaitForSeconds(timeBetweenStages);
            currentStageIndex++;
        }

        Debug.Log("All stages completed!");
    }

    void SpawnMonsters(Stage stage)
    {
        foreach (GameObject monsterPrefab in stage.monsters)
        {
            Vector3 spawnPosition = GetRandomSpawnPositionOnNavMesh();
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, spawnRotation);
            SetupMonster(monster);
            currentMonsters.Add(monster);
        }
    }

    void SetupMonster(GameObject monster)
    {
        WayPointPatrol patrol = monster.GetComponent<WayPointPatrol>();
        if (patrol != null)
        {
            patrol.playerTransform = playerTransform;
            patrol.waypoint = waypoints;
        }
    }

    Vector3 GetRandomSpawnPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += spawnCenter.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return spawnCenter.position; // ���� �� �⺻ ���� ��ġ
    }
}

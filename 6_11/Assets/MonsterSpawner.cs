using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public GameObject[] monsters; // 각 스테이지별로 스폰할 몬스터 배열
    }

    public Stage[] stages; // 모든 스테이지 정보
    public Transform spawnCenter; // 스폰 중심 위치
    public float spawnRadius = 50f; // 스폰 반경
    public float timeBetweenStages = 30f; // 각 스테이지 사이의 대기 시간
    public Transform playerTransform; // 플레이어 Transform 설정
    public Transform[] waypoints; // 웨이포인트 설정

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
            // 현재 스테이지의 몬스터 스폰
            SpawnMonsters(stages[currentStageIndex]);

            // 모든 몬스터가 죽을 때까지 대기
            while (currentMonsters.Count > 0)
            {
                currentMonsters.RemoveAll(monster => monster == null);
                yield return null;
            }

            // 다음 스테이지로 넘어가기 전 대기
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
        return spawnCenter.position; // 실패 시 기본 스폰 위치
    }
}

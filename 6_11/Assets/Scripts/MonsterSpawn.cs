using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // 몬스터 프리팹
    public Terrain terrain; // Terrain 객체
    public float spawnInterval = 5f; // 몬스터 생성 간격 (초 단위)
    public int maxMonsters = 10; // 최대 몬스터 수
    public float minDistanceFromTrees = 5f; // 나무와의 최소 거리

    private int currentMonsterCount = 0;
    private List<Vector3> treePositions = new List<Vector3>();

    void Start()
    {
        GetTreePositions();
        StartCoroutine(SpawnMonsters());
    }

    void GetTreePositions()
    {
        foreach (var treeInstance in terrain.terrainData.treeInstances)
        {
            Vector3 treeWorldPos = Vector3.Scale(treeInstance.position, terrain.terrainData.size) + terrain.transform.position;
            treePositions.Add(treeWorldPos);
        }
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            if (currentMonsterCount < maxMonsters)
            {
                SpawnMonster();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonster()
    {
        Vector3 spawnPosition;
        bool validPosition;

        do
        {
            float terrainWidth = terrain.terrainData.size.x;
            float terrainLength = terrain.terrainData.size.z;
            float terrainPosX = terrain.transform.position.x;
            float terrainPosZ = terrain.transform.position.z;

            float spawnPosX = terrainPosX + Random.Range(0, terrainWidth);
            float spawnPosZ = terrainPosZ + Random.Range(0, terrainLength);
            float spawnPosY = terrain.SampleHeight(new Vector3(spawnPosX, 0, spawnPosZ)) + terrain.transform.position.y;

            spawnPosition = new Vector3(spawnPosX, spawnPosY, spawnPosZ);

            validPosition = IsPositionValid(spawnPosition);

        } while (!validPosition);

        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        currentMonsterCount++;
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (var treePos in treePositions)
        {
            if (Vector3.Distance(position, treePos) < minDistanceFromTrees)
            {
                return false;
            }
        }
        return true;
    }
}

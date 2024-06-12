using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> monsterPrefabs; // ���� ������
    public Terrain terrain; // Terrain ��ü
    public float spawnInterval = 5f; // ���� ���� ���� (�� ����)
    public int maxMonsters = 10; // �ִ� ���� ��
    public float minDistanceFromTrees = 5f; // �������� �ּ� �Ÿ�
    public int simultaneousSpawnCount = 3; // �� ���� ������ ���� ��

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
                int spawnCount = Mathf.Min(simultaneousSpawnCount, maxMonsters - currentMonsterCount);
                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnMonster();
                }
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

        // ���� ������ ����Ʈ���� �������� �����Ͽ� ����
        GameObject selectedMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
        Instantiate(selectedMonsterPrefab, spawnPosition, Quaternion.identity);
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

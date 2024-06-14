using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampHP : MonoBehaviour
{
    [SerializeField]
    int campHP;

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        campHP -= damage;
        if (campHP <= 0)
        {
            campHP = 0;
            // 기지가 파괴되었을 때 처리
            Debug.Log("Camp is destroyed!");
        }
    }
}

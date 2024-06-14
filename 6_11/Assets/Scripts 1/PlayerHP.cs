using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private int playerHP = 100; // 기본 플레이어 체력 설정
    [SerializeField]
    private GameObject restartButton; // Restart 버튼을 할당할 필드

    void Start()
    {
        restartButton.SetActive(false); // 시작할 때 버튼 비활성화
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        Debug.Log($"플레이어 체력: {playerHP}");
        if (playerHP <= 0)
        {
            playerHP = 0;
            // 플레이어가 죽었을 때 처리
            Debug.Log("Player is dead!");
            restartButton.SetActive(true); // 체력이 0이 되었을 때 버튼 활성화
        }
    }

    // 인스펙터 창에서 체력을 실시간으로 업데이트하기 위한 메서드
    void OnValidate()
    {
        if (playerHP < 0)
        {
            playerHP = 0;
        }
    }

    // 플레이어 체력을 인스펙터에서 확인하기 위해 프로퍼티 추가
    public int GetPlayerHP()
    {
        return playerHP;
    }
}

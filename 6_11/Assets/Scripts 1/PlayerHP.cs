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
    [SerializeField]
    private Slider healthSlider; // 플레이어 HP를 표시할 슬라이더 UI
    [SerializeField]
    private Image fillImage; // 슬라이더의 Fill 이미지

    private int maxHP; // 최대 체력

    void Start()
    {
        maxHP = playerHP; // 최대 체력을 현재 체력으로 설정
        restartButton.SetActive(false); // 시작할 때 버튼 비활성화
        healthSlider.maxValue = maxHP; // 슬라이더의 최대값 설정
        healthSlider.value = playerHP; // 초기 슬라이더 값 설정
        healthSlider.interactable = false; // 슬라이더 비활성화
        UpdateHealthUI(); // 초기 HP UI 업데이트
    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        if (playerHP < 0)
        {
            playerHP = 0;
            // 플레이어가 죽었을 때 처리
            restartButton.SetActive(true); // 체력이 0이 되었을 때 버튼 활성화
        }
        UpdateHealthUI(); // 체력 변경 시 UI 업데이트
    }

    public void Heal(int amount)
    {
        playerHP += amount;
        if (playerHP > maxHP)
        {
            playerHP = maxHP;
        }
        UpdateHealthUI(); // 체력 변경 시 UI 업데이트
    }

    void UpdateHealthUI()
    {
        healthSlider.value = playerHP; // 슬라이더의 값 업데이트
        // 체력 비율에 따라 색상 변경
        float healthPercentage = (float)playerHP / maxHP;
        fillImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
}

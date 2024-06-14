using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private int playerHP = 100; // �⺻ �÷��̾� ü�� ����
    [SerializeField]
    private GameObject restartButton; // Restart ��ư�� �Ҵ��� �ʵ�
    [SerializeField]
    private Slider healthSlider; // �÷��̾� HP�� ǥ���� �����̴� UI
    [SerializeField]
    private Image fillImage; // �����̴��� Fill �̹���

    private int maxHP; // �ִ� ü��

    void Start()
    {
        maxHP = playerHP; // �ִ� ü���� ���� ü������ ����
        restartButton.SetActive(false); // ������ �� ��ư ��Ȱ��ȭ
        healthSlider.maxValue = maxHP; // �����̴��� �ִ밪 ����
        healthSlider.value = playerHP; // �ʱ� �����̴� �� ����
        healthSlider.interactable = false; // �����̴� ��Ȱ��ȭ
        UpdateHealthUI(); // �ʱ� HP UI ������Ʈ
    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        if (playerHP < 0)
        {
            playerHP = 0;
            // �÷��̾ �׾��� �� ó��
            restartButton.SetActive(true); // ü���� 0�� �Ǿ��� �� ��ư Ȱ��ȭ
        }
        UpdateHealthUI(); // ü�� ���� �� UI ������Ʈ
    }

    public void Heal(int amount)
    {
        playerHP += amount;
        if (playerHP > maxHP)
        {
            playerHP = maxHP;
        }
        UpdateHealthUI(); // ü�� ���� �� UI ������Ʈ
    }

    void UpdateHealthUI()
    {
        healthSlider.value = playerHP; // �����̴��� �� ������Ʈ
        // ü�� ������ ���� ���� ����
        float healthPercentage = (float)playerHP / maxHP;
        fillImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
}

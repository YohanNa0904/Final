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

    void Start()
    {
        restartButton.SetActive(false); // ������ �� ��ư ��Ȱ��ȭ
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        Debug.Log($"�÷��̾� ü��: {playerHP}");
        if (playerHP <= 0)
        {
            playerHP = 0;
            // �÷��̾ �׾��� �� ó��
            Debug.Log("Player is dead!");
            restartButton.SetActive(true); // ü���� 0�� �Ǿ��� �� ��ư Ȱ��ȭ
        }
    }

    // �ν����� â���� ü���� �ǽð����� ������Ʈ�ϱ� ���� �޼���
    void OnValidate()
    {
        if (playerHP < 0)
        {
            playerHP = 0;
        }
    }

    // �÷��̾� ü���� �ν����Ϳ��� Ȯ���ϱ� ���� ������Ƽ �߰�
    public int GetPlayerHP()
    {
        return playerHP;
    }
}

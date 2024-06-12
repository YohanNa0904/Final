using MyGameNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] public float HP = 100f;
    [SerializeField] public float MoveSpeed = 10f;
    public int scoreValue = 10; // ���Ͱ� �׾��� �� ��� ����
    public int goldValue = 5;   // ���Ͱ� �׾��� �� ��� ���

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // ���Ͱ� ���� ���� ���� (��: ���� ����, �ִϸ��̼� ��� ��)
        GameManager.Instance.AddScore(scoreValue);
        GameManager.Instance.AddGold(goldValue);
        Destroy(gameObject);
    }
}

using MyGameNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] public float HP = 100f;
    [SerializeField] public float MoveSpeed = 10f;
    public int scoreValue = 10; // 몬스터가 죽었을 때 얻는 점수
    public int goldValue = 5;   // 몬스터가 죽었을 때 얻는 골드

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
        // 몬스터가 죽을 때의 로직 (예: 몬스터 삭제, 애니메이션 재생 등)
        GameManager.Instance.AddScore(scoreValue);
        GameManager.Instance.AddGold(goldValue);
        Destroy(gameObject);
    }
}

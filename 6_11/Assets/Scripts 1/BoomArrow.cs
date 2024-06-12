using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomArrow : MonoBehaviour
{
    [SerializeField] private GameObject explosion; // 폭발 오브젝트
    [SerializeField] private float attackDamage = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 속도가 있는지 확인 (속도가 0인 경우 회전하지 않음)
        if (rb.velocity != Vector3.zero)
        {
            // Rigidbody의 운동 방향을 바라보게 회전 설정
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(attackDamage);

                // explosion 오브젝트를 소환
                Instantiate(explosion, transform.position, transform.rotation);

                // BoomArrow 오브젝트를 파괴
                Destroy(gameObject);
            }
        }
    }

    public void Shoot(float power)
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * power * 30f;
        }
    }
}

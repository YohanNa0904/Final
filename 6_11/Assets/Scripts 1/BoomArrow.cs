using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomArrow : MonoBehaviour
{
    [SerializeField] private GameObject explosion; // ���� ������Ʈ
    [SerializeField] private float attackDamage = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // �ӵ��� �ִ��� Ȯ�� (�ӵ��� 0�� ��� ȸ������ ����)
        if (rb.velocity != Vector3.zero)
        {
            // Rigidbody�� � ������ �ٶ󺸰� ȸ�� ����
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

                // explosion ������Ʈ�� ��ȯ
                Instantiate(explosion, transform.position, transform.rotation);

                // BoomArrow ������Ʈ�� �ı�
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

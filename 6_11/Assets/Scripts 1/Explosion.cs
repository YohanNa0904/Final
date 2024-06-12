using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // ������ ��
    [SerializeField] private float damageAmount = 20f;
    // �浹 ����
    [SerializeField] private float explosionRadius = 5f;

    // ��ũ��Ʈ�� Ȱ��ȭ�ǰ� �ִ� ���� �� �����Ӹ��� ȣ��˴ϴ�.
    private void Update()
    {
        // �ֺ��� �ִ� ��� ���͸� ã�Ƽ� �������� �ְ� �ڽ��� �����մϴ�.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                // �浹�� ���Ϳ��� �������� �ݴϴ�.
                Monster monster = collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDamage(damageAmount);
                }
            }
        }

        // ��ũ��Ʈ�� �ڽ��� �����մϴ�.
        Destroy(gameObject);
    }

    // ���� ������ �ð�ȭ�ϱ� ���� ����� ǥ���մϴ�.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

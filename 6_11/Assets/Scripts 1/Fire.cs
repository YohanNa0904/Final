using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // ������ ��
    [SerializeField] private float damageAmountPerSecond = 10f;
    // ������ ���� �ð�
    [SerializeField] private float damageDuration = 10f;
    // �浹 ����
    [SerializeField] private float fireRadius = 5f;

    // ��ũ��Ʈ�� Ȱ��ȭ�� �� ȣ��˴ϴ�.
    private void OnEnable()
    {
        // �������� �ִ� �ڷ�ƾ�� �����մϴ�.
        StartCoroutine(DealDamageOverTime());
    }

    private IEnumerator DealDamageOverTime()
    {
        float elapsed = 0f;

        // �������� ���� �ð� ���� �ֱ������� �ݴϴ�.
        while (elapsed < damageDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, fireRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    // �浹�� ���Ϳ��� �������� �ݴϴ�.
                    Monster monster = collider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.TakeDamage(damageAmountPerSecond);
                    }
                }
            }

            // 1�� ��ٸ��ϴ�.
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        // ������ ���� �ð��� ������ ������Ʈ�� �����մϴ�.
        Destroy(gameObject);
    }

    // �浹 ������ �ð�ȭ�ϱ� ���� ����� ǥ���մϴ�.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRadius);
    }
}

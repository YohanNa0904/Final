using UnityEngine;

public class Penetrate : MonoBehaviour
{
    [SerializeField] private float damageAmount = 100f;
    [SerializeField] private float maxPenetrationDistance = 100f;

    void Start()
    {
        // ������Ʈ�� ������ �� �ٷ� ���� �������� ���̸� �߻��Ͽ� ���Ϳ��� �������� �ݴϴ�.
        PenetrateMonsters();
    }

    void PenetrateMonsters()
    {
        // ������Ʈ�� ��ġ�� ���� �������� ���� ����
        Ray ray = new Ray(transform.position, transform.forward);

        // ���� ������ ��� ���͸� ����
        RaycastHit[] hits = Physics.RaycastAll(ray, maxPenetrationDistance, LayerMask.GetMask("Monster"));

        // ����� ���Ϳ��� �������� ��
        foreach (RaycastHit hit in hits)
        {
            Monster monster = hit.collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damageAmount);
            }
        }
    }
}
